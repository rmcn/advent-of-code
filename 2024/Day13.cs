
namespace AdventOfCode.Year2024;

public class Day13 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        return input.Lines().Batch(4).Sum(g => MinTokens(g.ToList()));
    }

    private int MinTokens(List<string> list)
    {
        if (list.Count < 3)
            return 0;

        var a = list[0].Ints();
        var b = list[1].Ints();
        var p = list[2].Ints();

        var min = int.MaxValue;

        for (int aPress = 0; aPress <= 100; aPress++)
        {
            for (int bPress = 0; bPress <= 100; bPress++)
            {
                if (a[0] * aPress + b[0] * bPress == p[0] && a[1] * aPress + b[1] * bPress == p[1])
                {
                    min = Min(min, aPress * 3 + bPress);
                }
            }
        }

        return min == int.MaxValue ? 0 : min;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
