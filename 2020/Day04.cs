
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020;

public class Day04 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        return input.Lines().Segment(IsBlank).Count(IsValidOne);
    }

    private Dictionary<string, Func<string, bool>> Fields = new()
    {
         { "byr", s => Regex.IsMatch(s, "^[0-9]{4}$") && s.Int() >= 1920 && s.Int() <= 2002 },
         { "iyr", s => Regex.IsMatch(s, "^[0-9]{4}$") && s.Int() >= 2010 && s.Int() <= 2020 },
         { "eyr", s => Regex.IsMatch(s, "^[0-9]{4}$") && s.Int() >= 2020 && s.Int() <= 2030 },
         { "hgt", s => (Regex.IsMatch(s, "^[0-9]+cm$") && s.Int() >= 150 && s.Int() <= 193) || (Regex.IsMatch(s, "^[0-9]+in$") && s.Int() >= 59 && s.Int() <= 76) },
         { "hcl", s => Regex.IsMatch(s, "^#[0-9a-f]{6}$") },
         { "ecl", s => "amb blu brn gry grn hzl oth".Split(' ').Contains(s) },
         { "pid", s => Regex.IsMatch(s, "^[0-9]{9}$") }
    };

    private bool IsValidOne(IEnumerable<string> lines)
    {
        Dictionary<string, string> values = ParsePassport(lines);

        return Fields.Keys.All(k => values.Keys.Contains(k));
    }

    private bool IsValidTwo(IEnumerable<string> lines)
    {
        Dictionary<string, string> values = ParsePassport(lines);

        return Fields.All(f => values.Keys.Contains(f.Key) && f.Value(values[f.Key]));
    }

    private static Dictionary<string, string> ParsePassport(IEnumerable<string> lines)
    {
        var values = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            var pairs = line.Split(' ');
            foreach (var pair in pairs.Where(IsNotBlank))
            {
                var keyVal = pair.Split(':');
                values.Add(keyVal[0], keyVal[1].Trim());
            }
        }

        return values;
    }

    public override Answer Two(string input)
    {
        return input.Lines().Segment(IsBlank).Count(IsValidTwo);
    }
}
