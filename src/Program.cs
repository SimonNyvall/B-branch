using Bbranch.Info;
using Bbranch.TablePrinter;
using Bbranch.TableData;
using Bbranch.ErrorHandling;

await BranchInfo.Initialize();

var gitPath = BranchInfo.GitPath;

Error.HandleGitDirNotFound(gitPath);

var workingBranch = BranchInfo.TryGetWorkingBranch(gitPath!);

Error.HandleNoWorkingBranch(workingBranch);

var branchTable = await Project.GitBranches(BranchInfo.GetNamesAndLastWirte(gitPath!));

Data.PrintBranchTable(branchTable);
