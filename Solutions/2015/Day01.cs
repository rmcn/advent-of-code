namespace AdventOfCode.Year2015;

public class Day01: IDay
{
    public object One(string input)
    {
        var up = input.Count(c => c == '(');
        var down = input.Count(c => c == ')');

        return up - down;
    }

    public object Two(string input)
    {
        int t = 0;

        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c == '(') t++;
            if (c == ')') t--;

            if (t == -1)
                return i + 1;
        }

        return t;
    }
}