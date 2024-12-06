namespace AdventOfCode;

public class Answer : object
{
    public Answer(object value)
    {
        Value = value;
    }

    public object Value { get; }

    public override string ToString() => Value.ToString() ?? string.Empty;

    public static implicit operator Answer(string v) => new(v);
    public static implicit operator Answer(int v) => new(v);
    public static implicit operator Answer(long v) => new(v);
    public static implicit operator Answer(decimal v) => new(v);
}

public class SubmitAnswer : Answer
{
    public SubmitAnswer(object value) : base(value)
    {
    }
}
