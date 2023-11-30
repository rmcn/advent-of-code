namespace AdventOfCode.Year2015;

public class Day01: IDay
{
    public (bool, string) One(string input)
    {
        var up = input.Count(c => c == '(');
        var down = input.Count(c => c == ')');

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