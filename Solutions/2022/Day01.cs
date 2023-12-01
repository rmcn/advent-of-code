namespace AdventOfCode.Year2022;

class Day01 : IDay
{
    public (bool, object) One(string input) => (true, Totals(input).OrderByDescending(c => c).First().ToString());

    public (bool, object) Two(string input) => (true, Totals(input).OrderByDescending(c => c).Take(3).Sum().ToString());

    private static List<int> Totals(string input) =>
        input
            .Lines()
            .Segment(IsBlank)
            .Select(e => e.Where(IsNotBlank).Select(Int).Sum())
            .ToList();
}