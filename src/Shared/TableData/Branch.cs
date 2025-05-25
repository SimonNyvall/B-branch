namespace Bbranch.Shared.TableData;

public sealed class Branch
{
    public string Name { get; set; }
    public bool IsWorkingBranch { get; set; }

    public Branch(string name, bool isWorkingBranch)
    {
        Name = name;
        IsWorkingBranch = isWorkingBranch;
    }
}