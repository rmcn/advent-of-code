namespace AdventOfCode.Year0000;

public class TemplateForDay00 : IDay
{
    public (bool, string) One(string input)
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


        return (false, t.ToString());
    }

    public (bool, string) Two(string input)
    {
        throw new NotImplementedException();
    }
}
