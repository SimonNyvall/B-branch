namespace Bbranch.Branch.TableData;

public record TableRow(int Ahead, int Behind, string BranchName, (string, string) LastCommit, bool IsWorkingBranch, string description);
