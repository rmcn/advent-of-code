
namespace AdventOfCode.Year2023;

public class Day12 : Solution
{
    public override string Example => @"";

    record Puzzle(string Springs, int[] Lengths);

    Puzzle ParsePuzzle(string s)
    {
        var parts = s.Split(' ');
        return new Puzzle(parts[0].Trim(), parts[1].Ints().ToArray());
    }

    public override Answer One(string input)
    {
        int t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzle).ToList();

        foreach (var p in puzzles)
        {
            var c = CountValid(p.Springs, p.Lengths);
            LogEx($"{p.Springs} count {c}");
            t += c;
        }

        return t;
    }

    string ReplaceFirst(string s, char r)
    {
        var i = s.IndexOf('?');
        var chars = s.ToCharArray();
        chars[i] = r;
        return new string(chars);
    }

    static char[] Splits = new char [] {'?', '.'};

    bool IsValid(string s, int[] lengths)
    {
        var parts = s.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != lengths.Length)
            return false;
        for(int i = 0; i < lengths.Length; i++)
        {
            if (parts[i].Length != lengths[i])
                return false;
        }
        return true;
    }

    bool IsPotential(string s, int[] lengths)
    {
        var broken = s.Count(c => c == '#');
        var unknown = s.Count(c => c == '?');
        var total = lengths.Sum();
        return broken <= total && broken + unknown >= total;
        /*
        var parts = s.Split(Splits, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < lengths.Length && i < parts.Length; i++)
        {
            if (parts[i].Length > lengths[i])
                return false;
        }
        return true;*/
    }

    private int CountValid(string springs, int[] lengths)
    {
        int t = 0;
        if (springs.Any(s => s == '?'))
        {
            if (!IsPotential(springs, lengths))
                return 0;
            
            t += CountValid(ReplaceFirst(springs, '.'), lengths);
            t += CountValid(ReplaceFirst(springs, '#'), lengths);
            return t;
        }
        else
        {
            return IsValid(springs, lengths) ? 1 : 0;
        }
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
