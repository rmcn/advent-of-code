namespace AdventOfCode.Year2019;

public class Day01 : IDay
{
    public (bool, string) One(string input)
    {
        decimal t = 0;

        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            decimal x = line.Int();
            t += Floor(x / 3) - 2;
        }

        return (true, t.ToString());
    }

    public (bool, string) Two(string input)
    {
        decimal t = 0;

        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            decimal x = line.Int();

            while (true)
            {
                var f = Floor(x / 3) - 2;
                if (f <= 0) break;
                t += f;
                x = f;
            }

        }

        return (true, t.ToString());
    }
}
