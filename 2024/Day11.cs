using System.Data.SqlTypes;

namespace AdventOfCode.Year2024;

public class Day11 : Solution
{
    public override string Example => @"125 17";

    public override Answer One(string input)
    {
        var l = new LinkedList<long>(input.Longs());
        for (int i = 0; i < 25; i++)
        {
            Blink(l);
        }
        return l.Count;
    }

    private void Blink(LinkedList<long> l)
    {
        var n = l.First;
        while (n != null)
        {
            var s = n.Value.ToString();

            if (s == "0")
            {
                var replacement = l.AddAfter(n, 1);
                l.Remove(n);
                n = replacement;
            }
            else if (s.Length % 2 == 0)
            {
                var replacement = l.AddAfter(n, long.Parse(s.Substring(s.Length / 2)));
                l.AddBefore(n, long.Parse(s.Substring(0, s.Length / 2)));
                l.Remove(n);
                n = replacement;
            }
            else
            {
                var replacement = l.AddAfter(n, n.Value * 2024);
                l.Remove(n);
                n = replacement;

            }

            n = n.Next;
        }
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
