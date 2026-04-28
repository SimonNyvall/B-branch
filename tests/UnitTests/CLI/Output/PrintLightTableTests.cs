using Bbranch.CLI.Output;
using Bbranch.Shared.TableData;
using FakeItEasy;

namespace Bbranch.Tests.CLI.Output;

public class PrintLightTableTests
{
    [Fact]
    public void Given_NoBranches_When_PrintLightTableRun_Then_Return_NoBranches()
    {
        var branches = new HashSet<GitBranch>();
        var pagerFake = A.Fake<IPager>();
        var sut = new PrintLightTable(pagerFake);

        sut.Print(branches, null);

        A.CallTo(() => pagerFake.StartLess(A<string>._, A<string>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Given_Brnaches_When_PrintLightTableRun_Then_PrintBranches()
    {
        var mainBranch = GitBranch.Default().SetBranch(new Branch("main", false));
        var branches = new HashSet<GitBranch> { mainBranch };

        var pagerFake = A.Fake<IPager>();
        var sut = new PrintLightTable(pagerFake);

        sut.Print(branches, null);

        A.CallTo(() => pagerFake.StartLess(A<string>._, A<string>._)).MustNotHaveHappened();
    }
}
