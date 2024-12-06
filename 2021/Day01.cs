using MoreLinq.Extensions;

namespace AdventOfCode.Year2021;

public class Day01 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var depths = input.Lines().Where(IsNotBlank).Select(s => s.Int()).ToList();
        return depths.Zip(depths.Skip(1)).Count(p => p.First < p.Second);
    }

    public override Answer Two(string input)
    {
        var depths = input.Lines().Where(IsNotBlank).Select(s => s.Int()).ToList();
        var trips = depths.Zip(depths.Skip(1)).Zip(depths.Skip(2)).Select(p1 => p1.First.First + p1.First.Second + p1.Second).ToList();
        return trips.Zip(trips.Skip(1)).Count(p => p.First < p.Second);
    }
}
