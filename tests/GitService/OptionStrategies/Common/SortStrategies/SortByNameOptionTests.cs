using Git.Options;
using Shared.TableData;

namespace Tests.GitService;

public class SortByNameOptionTests
{
    [Fact]
    public void SortByNameOption_ShouldReturnSortedBranches()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("b_feature", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("a_main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("c_feature", isWorkingBranch: false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result[0].Branch.Name);
        Assert.Equal("b_feature", result[1].Branch.Name);
        Assert.Equal("c_feature", result[2].Branch.Name);
    }

    [Fact]
    public void SortByNameOption_ShouldReturnSortedBranches_WhenBranchesAreAlreadySorted()
    {
        var branches = new List<GitBranch>
        {
            GitBranch.Default().SetBranch(new Branch("a_main", isWorkingBranch: true)),
            GitBranch.Default().SetBranch(new Branch("b_feature", isWorkingBranch: false)),
            GitBranch.Default().SetBranch(new Branch("c_feature", isWorkingBranch: false)),
        };

        var sortByNameOption = new SortByNameOptions();

        var result = sortByNameOption.Execute(branches);

        Assert.Equal("a_main", result[0].Branch.Name);
        Assert.Equal("b_feature", result[1].Branch.Name);
        Assert.Equal("c_feature", result[2].Branch.Name);
    }
}