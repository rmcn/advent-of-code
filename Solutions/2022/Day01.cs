namespace AdventOfCode.Year2022;

class Day01 : IDay
{
    public object One(string input) => Totals(input).OrderByDescending(c => c).First();

    public object Two(string input) => Totals(input).OrderByDescending(c => c).Take(3).Sum();

    private static List<int> Totals(string input) =>
        input
            .Lines()
            .Segment(IsBlank)
            .Select(e => e.Where(IsNotBlank).Select(Int).Sum())
            .ToList();
}