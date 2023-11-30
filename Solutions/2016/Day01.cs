namespace AdventOfCode.Year2016;

public class Day01 : IDay
{
    public (bool, string) One(string input)
    {
        var instructions = input.Split(',').Select(l => l.Trim()).Select(l => (l[0], l.Ints()[0])).ToList();

        var f = 0;

        var bn = 0;
        var be = 0;

        foreach(var (t, d) in instructions)
        {
            if (t == 'L') f--;
            if (t == 'R') f++;
            if (f == -1) f = 3;

            if (f % 4 == 0) bn += d;
            if (f % 4 == 1) be += d;
            if (f % 4 == 2) bn -= d;
            if (f % 4 == 3) be -= d;
        }

        return (true, (Abs(be) + Abs(bn)).ToString());
    }

    public (bool, string) Two(string input)
    {
        var instructions = input.Split(',').Select(l => l.Trim()).Select(l => (l[0], l.Ints()[0])).ToList();

        var f = 0;

        var bn = 0;
        var be = 0;

        var seen = new List<string>();

        seen.Add($"{bn},{be}");
        foreach(var (t, d) in instructions)
        {
            if (t == 'L') f--;
            if (t == 'R') f++;
            if (f == -1) f = 3;

            if (f % 4 == 0)
            {
                for(var i = 0; i < d; i++)
                {
                    bn++;
                    if (seen.Contains($"{bn},{be}"))
                        return (true, (Abs(be) + Abs(bn)).ToString());
                    seen.Add($"{bn},{be}");
                }
            }

            if (f % 4 == 1)
            {
                for(var i = 0; i < d; i++)
                {
                    be++;
                    if (seen.Contains($"{bn},{be}"))
                        return (true, (Abs(be) + Abs(bn)).ToString());
                    seen.Add($"{bn},{be}");
                }
            }

            if (f % 4 == 2)
            {
                for(var i = 0; i < d; i++)
                {
                    bn--;
                    if (seen.Contains($"{bn},{be}"))
                        return (true, (Abs(be) + Abs(bn)).ToString());
                    seen.Add($"{bn},{be}");
                }
            }

            if (f % 4 == 3)
            {
                for(var i = 0; i < d; i++)
                {
                    be--;
                    if (seen.Contains($"{bn},{be}"))
                        return (true, (Abs(be) + Abs(bn)).ToString());
                    seen.Add($"{bn},{be}");
                }
            }

        }

        return (true, (Abs(be) + Abs(bn)).ToString());
    }
}
