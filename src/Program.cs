using Bbranch.Git.Base.GitBase;
using Bbranch.TablePrinter;
using Bbranch.TableData;
using Bbranch.Git.Options.BranchOptions;
using Bbranch.Git.Options.ContainsOptions;
using Bbranch.Git.Options.TrackOptions;
using Bbranch.Git.Options.SortOptions;
using Cocona;

const string trackDescription = "Displays information about how many commits the specified branch is ahead or behind relative to its upstream branch.";
const string sortDescription = "Sorts the branches based on the specified criterion. Valid options are [date], [name], [ahead], or [behind].";
const string containsDescription = "Filters the list to only show branches that contain the specified string.";
const string noContainsDescription = "Filters the list to only show branches that do not contain the specified string.";
const string allDescription = "Displays all branches, both local and remote.";
const string remoteDescription = "Includes remote branches in the output.";
const string quietDescription = "Only displays the names of the branches without any additional information or formatting.";
const string versionDescription = "Shows the current version of the tool.";

CoconaApp.Run(async (
            [Option('t', Description = trackDescription)] string? track,
            [Option('q', Description = quietDescription)] bool? quiet,
            [Option('v', Description = versionDescription)] bool? version,
            [Option('s', Description = sortDescription)] string? sort,
            [Option('a', Description = allDescription)] bool? all,
            [Option('n', Description = noContainsDescription)] string? noContains,
            [Option('c', Description = containsDescription)] string? contains,
            [Option('r', Description = remoteDescription)] bool? remote
            ) =>
{
    validateOptions(contains, noContains, all, remote, quiet, version, sort, track);

    if (version == true)
    {
        Console.WriteLine("1.0.0");
        return;
    }

    var gitBase = await GitBase.Initialize();

    var branches = new List<GitBranch>();

    BranchOptions.GetBranches(all, remote, ref branches);

    ContainsOptions.GetBranches(contains, noContains, ref branches);

    var workingBranch = await gitBase.TryGetWorkingBranch();

    var branchTable = await TrackOptions.GetBranches(track, gitBase, branches, workingBranch);

    SortOptions.GetBranches(branchTable, sort, ref branchTable);

    Data.PrintBranchTable(branchTable, quiet ?? false);
});

void validateOptions(string? contains, string? noContains, bool? all, bool? remote, bool? quiet, bool? version, string? sort, string? track)
{
    if (version == true && (contains != null || noContains != null || all == true || remote == true || quiet == true || sort != null || track != null))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You cannot use --version with any other option");
        Console.ResetColor();
        Environment.Exit(1);
    }

    if (contains != null && noContains != null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You cannot use both --contains and --no-contains");
        Console.ResetColor();
        Environment.Exit(1);
    }

    if (all == true && remote == true)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("You cannot use both --all and --remote");
        Console.ResetColor();
        Environment.Exit(1);
    }
}

