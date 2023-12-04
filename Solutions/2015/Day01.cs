namespace AdventOfCode.Year2015;

public class Day01 : Solution
{
    public override string Example => ")())())";

    public override Answer One(string input)
    {
        var up = input.Count(c => c == '(');
        var down = input.Count(c => c == ')');

        return up - down;
    }

    public override Answer Two(string input)
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