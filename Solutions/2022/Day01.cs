namespace AdventOfCode.Year2022;

class Day01 : IDay
{
    public int Year => 2022;
    public int Day => 1;

    public string One(string input) => Totals(input).OrderByDescending(c => c).First().ToString();

    public string Two(string input) => Totals(input).OrderByDescending(c => c).Take(3).Sum().ToString();

    private static List<int> Totals(string input) =>
        input
            .Lines()
            .Segment(IsBlank)
            .Select(e => e.Where(IsNotBlank).Select(Int).Sum())
            .ToList();
}