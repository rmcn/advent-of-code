namespace AdventOfCode.Year0000;

public class Day00 : IDay
{
    public object One(string input)
    {
        int t = 0;

        var k = input.Lines().Where(IsNotBlank).Select((s, i) => s);

        foreach(var line in input.Lines().Where(IsNotBlank))
        {
        }

        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
        }


        return t;
    }

    public object Two(string input)
    {
        return 0;
    }
}
