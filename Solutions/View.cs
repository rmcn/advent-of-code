namespace AdventOfCode;

public class SubmitAnswer
{
    public SubmitAnswer(object value)
    {
        Value = value;
    }

    public object Value { get; }

    public override string ToString() => Value.ToString() ?? string.Empty;
}
