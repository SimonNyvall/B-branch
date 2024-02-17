namespace tests;

using Bbranch.Branch.Info;

public class BranchInfo_Tests
{
    [Fact]
    public void GetAheadBehind_ShouldReturn_TwoPositiveNumbers()
    {
        // Arrange
        var branchInfo = new BranchInfo();
        var gitPath = branchInfo.GitPath;

        if (gitPath is null) Assert.Fail("GitPath is null");

        // Act
        var result = branchInfo.GetAheadBehind(gitPath, "origin/main");

        // Assert
        Assert.True(result.Item1 >= 0);
        Assert.True(result.Item2 >= 0);
    }

    [Fact]
    public void GetNamesAndLastWrite_ShouldContain_MainBranchAndTimeStamp()
    {
        // Arrange
        var branchInfo = new BranchInfo();
        var gitPath = branchInfo.GitPath;

        if (gitPath is null) Assert.Fail("GitPath is null");

        // Act
        var result = branchInfo.GetNamesAndLastWirte(gitPath);
        var mainBranch = result.FirstOrDefault(x => x.Key == "main");

        // Assert
        Assert.NotNull(mainBranch);
        Assert.True(mainBranch.Value > DateTime.MinValue);
    }
}
