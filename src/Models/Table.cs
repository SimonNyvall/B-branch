namespace Bbranch.Table;

public record Table(int Ahead, int Behind, string Branch, (int, string) LastCommit);
