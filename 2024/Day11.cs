using System.Data.SqlTypes;

namespace AdventOfCode.Year2024;

public class Day11 : Solution
{
    public override string Example => @"125 17";

    public override Answer One(string input)
    {
        var l = new List<long>(input.Longs());
        return BlinkN(l, 25).Count;
    }

    private List<long> BlinkN(List<long> l, int n)
    {
        for (int i = 0; i < n; i++)
        {
            Blink(l);
        }
        return l;
    }

    private void Blink(List<long> l)
    {
        var count = l.Count;
        for (int i = 0; i < count; i++)
        {
            var s = l[i].ToString();

            if (s == "0")
            {
                l[i] = 1;
            }
            else if (s.Length % 2 == 0)
            {
                l[i] = long.Parse(s.Substring(s.Length / 2));
                l.Add(long.Parse(s.Substring(0, s.Length / 2)));
            }
            else
            {
                l[i] = l[i] * 2024;
            }
        }
    }

    public override Answer Two(string input)
    {
        var memo5Step = new Dictionary<long, Dictionary<long, long>>();

        var counts = GetCounts(input.Longs());

        for (int mi = 0; mi < 15; mi++) // 15 * 5 step = 75
        {
            var newCounts = new Dictionary<long, long>();
            foreach (var kvp in counts)
            {
                if (!memo5Step.ContainsKey(kvp.Key))
                {
                    memo5Step[kvp.Key] = GetCounts(BlinkN(new List<long>(new [] {kvp.Key}), 5));
                }

                var children = memo5Step[kvp.Key];
                foreach (var child in children)
                {
                    var prev = newCounts.TryGetValue(child.Key, out var x) ? x : 0; 
                    newCounts[child.Key] = prev + child.Value * kvp.Value;
                }
            }
            counts = newCounts;
        }

        return counts.Sum(kvp => kvp.Value);
    }

    private static Dictionary<long, long> GetCounts(List<long> nums)
    {
        return nums.GroupBy(l => l).ToDictionary(g => g.Key, g => g.LongCount());
    }
}
