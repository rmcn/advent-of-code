
namespace AdventOfCode.Year2020;

public class Day04 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        return input.Lines().Segment(IsBlank).Count(IsValid);

    }

    private string[] expectedKeys = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

    private bool IsValid(IEnumerable<string> lines)
    {
        var keys = new HashSet<string>();
        foreach (var line in lines)
        {
            var pairs = line.Split(' ');
            foreach (var pair in pairs)
            {
                var key = pair.Split(':')[0];
                keys.Add(key);
            }
        }

        return expectedKeys.All(k => keys.Contains(k));
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
