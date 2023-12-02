namespace AdventOfCode;

public abstract class Solution
{
    public int Year => this.GetType().FullName!.Ints()[0];
    public int Day => this.GetType().FullName!.Ints()[1];

    public abstract object One(string input);
    public abstract object Two(string input);
}