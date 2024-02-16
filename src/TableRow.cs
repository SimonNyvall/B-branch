namespace Bbranch.Branch.TableData;

public record TableRow(int Ahead, int Behind, string BranchName, (int, string) LastCommit);
