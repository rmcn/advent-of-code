namespace AdventOfCode.Year2022;

public class Day07 : IDay
{
    public (bool, object) One(string input)
    {
        var path = new Stack<string>();
        var sizes = new Dictionary<string, int>();
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            if (line == "$ cd ..")
            {
                path.Pop();
            }
            else if (line.StartsWith("$ cd "))
            {
                path.Push(line.Substring(5));
                sizes.Add(string.Join('/', path.Reverse()), 0);
            }

            if (char.IsDigit(line[0]))
            {
                sizes[string.Join('/', path.Reverse())] = sizes[string.Join('/', path.Reverse())] + line.Ints()[0];
            }
        }

        var a = 0;
        foreach(var d in sizes.Keys)
        {
            var t = sizes.Keys.Where(k => k.StartsWith(d)).Select(k => sizes[k]).Sum();
            if (t <= 100000)
            {
                a += t;
            }
        }

        return (true, a.ToString());
    }

    public (bool, object) Two(string input)
    {
        var path = new Stack<string>();
        var sizes = new Dictionary<string, int>();
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            if (line == "$ cd ..")
            {
                path.Pop();
            }
            else if (line.StartsWith("$ cd "))
            {
                path.Push(line.Substring(5));
                sizes.Add(string.Join('/', path.Reverse()), 0);
            }

            if (char.IsDigit(line[0]))
            {
                sizes[string.Join('/', path.Reverse())] = sizes[string.Join('/', path.Reverse())] + line.Ints()[0];
            }
        }

        var totals = new Dictionary<string, int>();

        foreach(var d in sizes.Keys)
        {
            var dt = sizes.Keys.Where(k => k.StartsWith(d)).Select(k => sizes[k]).Sum();
            totals.Add(d, dt);
        }

        var t = 30000000 - (70000000 - totals["/"]);

        var a = totals.OrderBy(kvp => kvp.Value).Where(kvp => kvp.Value >= t).First().Value;

        return (true, a.ToString());
    }
}
