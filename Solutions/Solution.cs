namespace AdventOfCode;

public abstract class Solution
{
    public int Year => this.GetType().FullName!.Ints()[0];
    public int Day => this.GetType().FullName!.Ints()[1];

    public abstract object One(string input);
    public abstract object Two(string input);

    public virtual string Example => string.Empty;

    /// <summary>Log a message on all runs.</summary>
    public void Log(string message) => Console.WriteLine(message);
    /// <summary>Log a message but only on example runs.</summary>
    public Action<string> LogEx { get; set; } = (string m) => {};
}