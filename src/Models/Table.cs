namespace Bbranch.Table;

public record Table(int Ahead, int Behind, string BranchName, (int, string) LastCommit);
