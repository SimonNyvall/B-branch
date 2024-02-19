using Bbranch.Info;
using Bbranch.TablePrinter;
using Bbranch.TableData;
using Bbranch.ErrorHandling;

var branchInfo = new BranchInfo();

var gitPath = branchInfo.GitPath;

Error.HandleGitDirNotFound(gitPath);

var workingBranch = branchInfo.TryGetWorkingBranch(gitPath!);

Error.HandleNoWorkingBranch(workingBranch);

var branchTable = Project.GitBranches(branchInfo.GetNamesAndLastWirte(gitPath!));

Data.PrintBranchTable(branchTable);
