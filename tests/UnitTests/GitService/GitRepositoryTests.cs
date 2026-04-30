using System.IO.Compression;
using System.Text;
using Bbranch.GitService;
using Bbranch.GitService.Base;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.GitService;

[Collection("seq")]
public class GitRepositoryTests
{
    private readonly IIOAbstration _iOAbstration;
    private readonly DirectoryInfo _directoryInfo;

    public GitRepositoryTests()
    {
        Reset();
        _iOAbstration = A.Fake<IIOAbstration>();
        _directoryInfo = new DirectoryInfo("path");

        A.CallTo(() => _iOAbstration.DirectoryExists(A<string>._)).Returns(true);
        A.CallTo(() => _iOAbstration.FileExists(A<string>._)).Returns(false);
        A.CallTo(() => _iOAbstration.GetCurrentDirectory()).Returns(_directoryInfo);

        A.CallTo(() => _iOAbstration.FileExists(Path.Combine(_directoryInfo.FullName, "HEAD")))
            .Returns(false);
        A.CallTo(() =>
                _iOAbstration.DirectoryExists(Path.Combine(_directoryInfo.FullName, "objects"))
            )
            .Returns(false);
        A.CallTo(() => _iOAbstration.DirectoryExists(Path.Combine(_directoryInfo.FullName, "refs")))
            .Returns(false);
    }

    private static void Reset()
    {
        GitRepository._instance = null;
        GitRepository._gitPath = string.Empty;
        GitRepository._isWorktreeRepo = false;
    }

    [Fact]
    public async Task GetInstance_ShouldInitializeSingleton_WhenCalledFirstTime()
    {
        var firstInstance = await GitRepository.GetInstance(_iOAbstration);
        var secondInstance = await GitRepository.GetInstance(_iOAbstration);

        Assert.Same(firstInstance, secondInstance);
    }

    [Fact]
    public async Task SetGitPath_ShouldFindDotGitDirectory_InParentHierarchy()
    {
        var currentPath = Path.Combine(_directoryInfo.FullName, ".git");
        var parentPath = Path.Combine((_directoryInfo.Parent?.FullName) ?? string.Empty, ".git");

        A.CallTo(() => _iOAbstration.GetCurrentDirectory()).Returns(_directoryInfo);
        A.CallTo(() => _iOAbstration.DirectoryExists(currentPath)).Returns(false);
        A.CallTo(() => _iOAbstration.DirectoryExists(parentPath)).Returns(true);

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);
        Assert.Equal(parentPath, GitRepository._gitPath);
    }

    [Fact]
    public async Task SetGitPath_ShouldDetectWorktreeRepository_WhenStructureMatches()
    {
        var dotGitPath = Path.Combine(_directoryInfo.FullName, ".git");

        A.CallTo(() => _iOAbstration.GetCurrentDirectory()).Returns(_directoryInfo);
        A.CallTo(() => _iOAbstration.DirectoryExists(dotGitPath)).Returns(false);
        A.CallTo(() => _iOAbstration.FileExists(dotGitPath)).Returns(false);
        A.CallTo(() => _iOAbstration.FileExists(Path.Combine(_directoryInfo.FullName, "HEAD")))
            .Returns(true);
        A.CallTo(() =>
                _iOAbstration.DirectoryExists(Path.Combine(_directoryInfo.FullName, "objects"))
            )
            .Returns(true);
        A.CallTo(() => _iOAbstration.DirectoryExists(Path.Combine(_directoryInfo.FullName, "refs")))
            .Returns(true);

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);
        Assert.True(GitRepository._isWorktreeRepo);
    }

    [Fact]
    public async Task GetWorkingBranch_ShouldReturnBranchName_WhenHeadContainsRef()
    {
        A.CallTo(() => _iOAbstration.ReadAllText(A<string>._)).Returns("ref: refs/heads/master");

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);

        var workingBranchName = await gitRepository.GetWorkingBranch();

        Assert.Equal("master", workingBranchName);
    }

    [Fact]
    public async Task GetWorkingBranch_ShouldReturnEmtpyString_WhenHeadDoesNotContainsRef()
    {
        A.CallTo(() => _iOAbstration.ReadAllText(A<string>._)).Returns("heads/master");

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);

        var workingBranchName = await gitRepository.GetWorkingBranch();

        Assert.Equal(string.Empty, workingBranchName);
    }

    [Fact]
    public async Task GetWorkingBranch_ShouldReturnEmtpyString_WhenHeadIsDetached()
    {
        A.CallTo(() => _iOAbstration.ReadAllText(A<string>._))
            .Returns("c3c88501151013b69bae2a49bbcfc6b6aa85d1c9");

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);

        var workingBranchName = await gitRepository.GetWorkingBranch();

        Assert.Equal(string.Empty, workingBranchName);
    }

    [Fact]
    public async Task GetWorkingBranch_ShouldHandleNestedBranchNames()
    {
        A.CallTo(() => _iOAbstration.ReadAllText(A<string>._))
            .Returns("ref: refs/heads/feature/feature-1");

        var gitRepository = await GitRepository.GetInstance(_iOAbstration);

        Assert.NotNull(gitRepository);

        var workingBranchName = await gitRepository.GetWorkingBranch();

        Assert.Equal("feature/feature-1", workingBranchName);
    }

    [Fact]
    public async Task GetLastCommit_ShouldReturnFileWriteTime_WhenObjectExists()
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var commitHashPath = Path.Combine(GitRepository._gitPath, "refs", "heads", "main");
        var lastWriteTime = DateTime.Now;

        A.CallTo(() => _iOAbstration.ReadAllText(commitHashPath))
            .Returns("c3c88501151013b69bae2a49bbcfc6b6aa85d1c9");

        A.CallTo(() => _iOAbstration.DirectoryExists(A<string>._)).Returns(true);

        A.CallTo(() => _iOAbstration.GetLastWriteTime(A<string>._)).Returns(lastWriteTime);

        var actual = await gitRepository.GetLastCommitDate("main");

        Assert.Equal(lastWriteTime, actual);
    }

    [Fact]
    public async Task GetRemoteAheadBehind_ShouldReturnAheadBehind_WhenComputationSucceeds()
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var remoteBranchName = "remote/branch";
        var commitHash = "c3c88501151013b69bae2a49bbcfc6b6aa85d1c9";
        var branchRefBytes = Encoding.UTF8.GetBytes(commitHash);
        var dirName = commitHash[..2];
        var fileName = commitHash[2..];
        var commitObjectPath = Path.Combine(GitRepository._gitPath, "objects", dirName, fileName);

        A.CallTo(() => _iOAbstration.FileExists(remoteBranchName)).Returns(true);
        A.CallTo(() => _iOAbstration.ReadAllBytes(A<string>._)).Returns(branchRefBytes);
        A.CallTo(() => _iOAbstration.FileExists(commitObjectPath)).Returns(false);

        var result = await gitRepository.GetRemoteAheadBehind("master", "origin/master");

        Assert.Equal(0, result.Ahead);
        Assert.Equal(0, result.Behind);
    }

    [Fact]
    public async Task GetLocalAheadBehind_ShouldReturnAheadBehind_WhenComputationSucceeds()
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var remoteBranchName = "remote/branch";
        var commitHash = "c3c88501151013b69bae2a49bbcfc6b6aa85d1c9";
        var branchRefBytes = Encoding.UTF8.GetBytes(commitHash);
        var dirName = commitHash[..2];
        var fileName = commitHash[2..];
        var commitObjectPath = Path.Combine(GitRepository._gitPath, "objects", dirName, fileName);

        A.CallTo(() => _iOAbstration.FileExists(remoteBranchName)).Returns(true);
        A.CallTo(() => _iOAbstration.ReadAllBytes(A<string>._)).Returns(branchRefBytes);
        A.CallTo(() => _iOAbstration.FileExists(commitObjectPath)).Returns(false);

        var result = await gitRepository.GetLocalAheadBehind("master");

        Assert.Equal(0, result.Ahead);
        Assert.Equal(0, result.Behind);
    }

    [Fact]
    public async Task GetAheadBehind_ShouldReturnCorrectAheadBehind_WhenLocalAheadByOne()
    {
        var repo = GitRepository.GetInstanceForTests(_iOAbstration);

        var localBranch = "main";

        // Commit hashes
        var commitA = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var commitB = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        var commitC = "cccccccccccccccccccccccccccccccccccccccc";

        // Paths
        var localRefPath = Path.Combine(GitRepository._gitPath, "refs", "heads", localBranch);
        var remoteRefPath = Path.Combine(
            GitRepository._gitPath,
            "refs",
            "remotes",
            "origin",
            localBranch
        );

        // Local = C, Remote = B
        A.CallTo(() => _iOAbstration.FileExists(remoteRefPath)).Returns(true);

        A.CallTo(() => _iOAbstration.ReadAllBytes(localRefPath))
            .Returns(Encoding.UTF8.GetBytes(commitC));

        A.CallTo(() => _iOAbstration.ReadAllBytes(remoteRefPath))
            .Returns(Encoding.UTF8.GetBytes(commitB));

        // Mock object existence
        A.CallTo(() => _iOAbstration.FileExists(A<string>.That.Contains("objects"))).Returns(true);

        // Mock file names
        A.CallTo(() => _iOAbstration.GetFileName(A<string>.That.Contains(commitC.Substring(2))))
            .Returns(commitC);

        A.CallTo(() => _iOAbstration.GetFileName(A<string>.That.Contains(commitB.Substring(2))))
            .Returns(commitB);

        A.CallTo(() => _iOAbstration.GetFileName(A<string>.That.Contains(commitA.Substring(2))))
            .Returns(commitA);

        // Provide compressed commit objects
        A.CallTo(() => _iOAbstration.ReadAllBytes(A<string>.That.Contains(commitC.Substring(2))))
            .Returns(CreateCompressedCommit($"parent {commitB}\n"));

        A.CallTo(() => _iOAbstration.ReadAllBytes(A<string>.That.Contains(commitB.Substring(2))))
            .Returns(CreateCompressedCommit($"parent {commitA}\n"));

        A.CallTo(() => _iOAbstration.ReadAllBytes(A<string>.That.Contains(commitA.Substring(2))))
            .Returns(CreateCompressedCommit("")); // root commit

        var result = await repo.GetAheadBehind(localBranch);

        // Assert
        Assert.Equal(1, result.Ahead);
        Assert.Equal(2, result.Behind);
    }

    [Theory]
    [InlineData("1 1")]
    [InlineData("1      1")]
    [InlineData("155 155")]
    public async Task ParseAheadBehind_ShouldReturnAheadBehind_WhenGivenString(string input)
    {
        var expectedAhead = input.Split(' ').First(x => int.TryParse(x, out _));
        var exptectedBehind = input.Split(' ').Last(x => int.TryParse(x, out _));

        var result = GitRepository.ParseAheadBehind(input);

        Assert.Equal(expectedAhead, result.Ahead.ToString());
        Assert.Equal(exptectedBehind, result.Behind.ToString());
    }

    [Fact]
    public async Task GetLocalBranchNames_ShouldReturnBranches_FromHeadsDirectory()
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var headsPath = Path.Combine(GitRepository._gitPath, "refs", "heads");
        var masterBranch = "master";
        var featureBranch = "feature/feature-1";
        var localBranches = new string[] { masterBranch, featureBranch };
        var packedRefPath = Path.Combine(GitRepository._gitPath, "packed-refs");
        var packedRefContent = new string[]
        {
            "# some comment here",
            "   ",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/heads/master",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/heads/feature/feature-1",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/heads/some-packed-branch",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/heads/feat/some-packed-branch-feat",
        };
        var headPath = Path.Combine(GitRepository._gitPath, "HEAD");

        A.CallTo(() => _iOAbstration.DirectoryExists(headsPath)).Returns(true);
        A.CallTo(() => _iOAbstration.GetFiles(A<string>._)).Returns(localBranches);
        A.CallTo(() => _iOAbstration.GetRelativePath(A<string>._, masterBranch))
            .Returns(masterBranch);
        A.CallTo(() => _iOAbstration.GetRelativePath(A<string>._, featureBranch))
            .Returns(featureBranch);
        A.CallTo(() => _iOAbstration.FileExists(packedRefPath)).Returns(true);
        A.CallTo(() => _iOAbstration.ReadAllLines(packedRefPath)).Returns(packedRefContent);
        A.CallTo(() => _iOAbstration.ReadAllText(headPath))
            .Returns("c3c88501151013b69bae2a49bbcfc6b6aa85d1c9");

        var result = await gitRepository.GetLocalBranchNames();
        var branchNames = result.Select(x => x.Branch.Name);

        Assert.Collection(
            branchNames,
            b => Assert.Equal("master", b),
            b => Assert.Equal("feature/feature-1", b),
            b => Assert.Equal("some-packed-branch", b),
            b => Assert.Equal("feat/some-packed-branch-feat", b),
            b => Assert.Equal("(HEAD detached at c3c8850)", b)
        );

        Assert.All(result, (b) => Assert.False(b.IsRemote));

        Assert.Equal("c3c8850", result.Last().DetachedHead.commitHash);

        Assert.All(result.SkipLast(1), (b) => Assert.False(b.IsSymbolic));
    }

    [Fact]
    public async Task GetRemoteBranchNames_ShouldReturnBranches_FromRemotesDirectory()
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var remotePath = Path.Combine(GitRepository._gitPath, "refs", "remotes");
        var masterBranch = "origin/master";
        var featureBranch = "origin/feature/feature-1";
        var localBranches = new string[] { masterBranch, featureBranch };
        var packedRefPath = Path.Combine(GitRepository._gitPath, "packed-refs");
        var packedRefContent = new string[]
        {
            "# some comment here",
            "   ",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/remotes/origin/master",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/remotes/origin/feature/feature-1",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/remotes/origin/some-packed-branch",
            "aaa955733eee66e1d0d0f670e5cc1281adaffb52 refs/remotes/origin/feat/some-packed-branch-feat",
        };
        var headPath = Path.Combine(GitRepository._gitPath, "HEAD");

        A.CallTo(() => _iOAbstration.DirectoryExists(remotePath)).Returns(true);
        A.CallTo(() => _iOAbstration.GetFiles(A<string>._)).Returns(localBranches);
        A.CallTo(() => _iOAbstration.GetRelativePath(A<string>._, masterBranch))
            .Returns(masterBranch);
        A.CallTo(() => _iOAbstration.GetRelativePath(A<string>._, featureBranch))
            .Returns(featureBranch);
        A.CallTo(() => _iOAbstration.FileExists(packedRefPath)).Returns(true);
        A.CallTo(() => _iOAbstration.ReadAllLines(packedRefPath)).Returns(packedRefContent);

        var result = await gitRepository.GetRemoteBranchNames();
        var branchNames = result.Select(x => x.Branch.Name);

        Assert.Collection(
            branchNames,
            b => Assert.Equal("origin/master", b),
            b => Assert.Equal("origin/feature/feature-1", b),
            b => Assert.Equal("origin/some-packed-branch", b),
            b => Assert.Equal("origin/feat/some-packed-branch-feat", b)
        );

        Assert.All(result, (b) => Assert.True(b.IsRemote));
    }

    [Theory]
    [MemberData(nameof(GetBranchDescriptionTestData))]
    public async Task GetBranchDescription_ShouldParseDescriptionsCorrectly(
        bool fileExists,
        string[] fileLines,
        HashSet<GitBranch> branches,
        Dictionary<string, string> expectedDescriptions
    )
    {
        var gitRepository = await GitRepository.GetInstance(_iOAbstration);
        Assert.NotNull(gitRepository);

        var descriptionPath = Path.Combine(GitRepository._gitPath, "EDIT_DESCRIPTION");

        A.CallTo(() => _iOAbstration.FileExists(descriptionPath)).Returns(fileExists);

        if (fileExists)
        {
            A.CallTo(() => _iOAbstration.ReadAllLines(descriptionPath)).Returns(fileLines);
        }

        var result = await gitRepository.GetBranchDescription(branches);

        foreach (var branch in result)
        {
            if (expectedDescriptions.TryGetValue(branch.Branch.Name, out var expected))
            {
                Assert.Equal(expected, branch.Description);
            }
            else
            {
                Assert.True(string.IsNullOrEmpty(branch.Description));
            }
        }
    }

    public static IEnumerable<object[]> GetBranchDescriptionTestData()
    {
        yield return new object[]
        {
            // description file exists
            true,
            // file lines
            new[] { "[main]", "Main branch description" },
            // input branches
            new HashSet<GitBranch> { GitBranch.Default().SetBranch(new Branch("main", false)) },
            // expected descriptions
            new Dictionary<string, string> { { "main", "Main branch description" } },
        };

        yield return new object[]
        {
            true,
            new[] { "[feature/test]", "Line one", "Line two" },
            new HashSet<GitBranch>
            {
                GitBranch.Default().SetBranch(new Branch("feature/test", false)),
            },
            new Dictionary<string, string> { { "feature/test", "Line one Line two" } },
        };

        yield return new object[]
        {
            true,
            new[] { "# comment", "", "[dev]", "Dev branch" },
            new HashSet<GitBranch> { GitBranch.Default().SetBranch(new Branch("dev", false)) },
            new Dictionary<string, string> { { "dev", "Dev branch" } },
        };

        yield return new object[]
        {
            true,
            new[] { "[main]", "Main desc", "[dev]", "Dev desc" },
            new HashSet<GitBranch>
            {
                GitBranch.Default().SetBranch(new Branch("main", false)),
                GitBranch.Default().SetBranch(new Branch("dev", false)),
            },
            new Dictionary<string, string> { { "main", "Main desc" }, { "dev", "Dev desc" } },
        };

        yield return new object[]
        {
            // file does NOT exist
            false,
            Array.Empty<string>(),
            new HashSet<GitBranch> { GitBranch.Default().SetBranch(new Branch("main", false)) },
            new Dictionary<string, string>(), // no changes expected
        };
    }

    private static byte[] CreateCompressedCommit(string content)
    {
        var inputBytes = Encoding.UTF8.GetBytes(content);

        using var output = new MemoryStream();
        using (var zlib = new ZLibStream(output, CompressionMode.Compress, true))
        {
            zlib.Write(inputBytes, 0, inputBytes.Length);
        }

        return output.ToArray();
    }
}
