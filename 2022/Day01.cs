namespace AdventOfCode.Year2022;

public class Day01 : Solution
{
    public override Answer One(string input) => Totals(input).OrderByDescending(c => c).First();

    public override Answer Two(string input) => Totals(input).OrderByDescending(c => c).Take(3).Sum();

    private static List<int> Totals(string input) =>
        input
            .Lines()
            .Segment(IsBlank)
            .Select(e => e.Where(IsNotBlank).Select(Int).Sum())
            .ToList();
}