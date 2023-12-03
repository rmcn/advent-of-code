namespace AdventOfCode.Year2018;

public class Day01 : Solution
{
    public override object One(string input)
    {
        int t = 0;


        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            t += line.Int();
        }

        return t;
    }

    public override object Two(string input)
    {
        int t = 0;

        var seen = new List<int>();
        seen.Add(t);

        while (true)
        {
            foreach (var line in input.Lines().Where(IsNotBlank))
            {
                t += line.Int();

                if (seen.Contains(t))
                {
                    return t;
                }
                seen.Add(t);
            }
        }

        throw new Exception();
    }
}
