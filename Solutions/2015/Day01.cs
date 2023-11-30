namespace AdventOfCode.Year2015;

public class Day01: IDay
{
    public (bool, string) One(string input)
    {
        int t = 0;

        var up = input.Trim().Select(c => c).Where(c => c == '(').Count();
        var down = input.Trim().Select(c => c).Where(c => c == ')').Count();

        return (true, (up - down).ToString());
    }

    public (bool, string) Two(string input)
    {
        int t = 0;

        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c == '(') t++;
            if (c == ')') t--;

            if (t == -1)
                return (true, (i + 1).ToString());
        }

        return (false, t.ToString());
    }
}