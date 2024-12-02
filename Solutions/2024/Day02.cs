

using MoreLinq.Extensions;

namespace AdventOfCode.Year2024;

public class Day02 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {

        return input.Lines().Where(IsNotBlank).Count(l => IsSafe(l.Ints()));

    }

    private bool IsSafe(IEnumerable<int> vals)
    {
        var deltas = vals.Zip(vals.Skip(1)).Select(p => Sign(p.First - p.Second)).Distinct();
        return deltas.Count() == 1 && deltas.Single() != 0 && vals.Zip(vals.Skip(1)).All(p => Abs(p.First - p.Second) <= 3);
    }

    public override Answer Two(string input)
    {
        return input.Lines().Where(IsNotBlank).Count(IsSafeTwo);
    }

    private bool IsSafeTwo(string arg)
    {
        var vals = arg.Ints();
        if (IsSafe(vals)) return true;

        for (var i = 0; i < vals.Count; i++)
        {
            if (IsSafe(vals.Where((v, ix) => ix != i)))
                return true;

        }
        return false;
    }
}
