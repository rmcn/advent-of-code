namespace AdventOfCode.Year2017;

public class Day01 : IDay
{
    public (bool, object) One(string input)
    {
        int t = 0;
        var s = input.Trim();

        for (int i = 0; i < s.Length; i++)
        {
            var n = (i + 1) % s.Length;

            if (s[i] == s[n])
                t += s[i].ToString().Int();
        }

        return (true, t.ToString());
    }

    public (bool, object) Two(string input)
    {
        int t = 0;
        var s = input.Trim();

        for (int i = 0; i < s.Length; i++)
        {
            var n = (i + s.Length/2) % s.Length;

            if (s[i] == s[n])
                t += s[i].ToString().Int();
        }

        return (true, t.ToString());
    }
}
