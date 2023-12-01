namespace AdventOfCode.Year2018;

public class Day01 : IDay
{
    public object One(string input)
    {
        int t = 0;


        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            t += line.Ints()[0];
        }

        return t;
    }

    public object Two(string input)
    {
        int t = 0;

        var seen = new List<int>();
        seen.Add(t);

        while(true)
        {
            foreach(var line in input.Lines().Where(IsNotBlank))
            {
                t += line.Ints()[0];

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
