namespace Bbranch.Shared.TableData;

public struct Branch
{
    public string Name { get; set; }
    public bool IsWorkingBranch { get; set; }

    public Branch(string name, bool isWorkingBranch)
    {
        Name = name;
        IsWorkingBranch = isWorkingBranch;
    }
}