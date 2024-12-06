namespace AdventOfCode;

public abstract class Solution
{
    public int Year => this.GetType().FullName!.Ints()[0];
    public int Day => this.GetType().FullName!.Ints()[1];

    public virtual string FilePath => string.Empty;
    public DateTime LastModified =>
        this.FilePath == string.Empty
            ? DateTime.MinValue
            : File.GetLastWriteTimeUtc(this.FilePath);

    public abstract Answer One(string input);
    public abstract Answer Two(string input);

    public virtual string Example => string.Empty;

    /// <summary>Log a message on all runs.</summary>
    public void Log(string message) => Console.WriteLine(message);
    /// <summary>Log a message but only on example runs.</summary>
    public Action<string> LogEx { get; set; } = (string m) => { };
}