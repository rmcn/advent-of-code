
namespace AdventOfCode.Year2020;

public class Day06 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        return input.Lines().Segment(IsBlank).Sum(CountAnyoneYes);
    }

    private int CountAnyoneYes(IEnumerable<string> enumerable)
    {
        return string.Join("", enumerable.Select(s => s.Trim())).Distinct().Count();
    }

    public override Answer Two(string input)
    {
        return input.Lines().Segment(IsBlank).Sum(CountEveryoneYes);
    }

    private int CountEveryoneYes(IEnumerable<string> enumerable)
    {
        var people = enumerable.Where(IsNotBlank).ToList();
        var answers = string.Join("", people.Select(s => s.Trim())).Distinct().ToArray();
        return answers.Count(a => people.All(p => p.Contains(a)));
    }
}
