using Bbranch.CLI.Output;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.CLI.Output;

public class PrintFullTableTests
{
    [Fact]
    public void Given_NoBranches_When_PrintFullTableRun_Then_Return_NoBranches()
    {
        var branches = new HashSet<GitBranch>();
        var pagerFake = A.Fake<IPager>();
        var sut = new PrintFullTable(pagerFake);

        sut.Print(branches, null);

        A.CallTo(() => pagerFake.StartLess(A<string>.Ignored, A<string>.Ignored))
            .MustNotHaveHappened();
    }

    [Fact]
    public void Given_Brnaches_When_PrintFullTableRun_Then_PrintBranches()
    {
        var mainBranch = GitBranch.Default().SetBranch(new Branch("main", false));
        var branches = new HashSet<GitBranch> { mainBranch };

        var pagerFake = A.Fake<IPager>();
        var sut = new PrintFullTable(pagerFake);

        sut.Print(branches, null);

        A.CallTo(() => pagerFake.StartLess(A<string>.Ignored, A<string>.Ignored))
            .MustNotHaveHappened();
    }
}
