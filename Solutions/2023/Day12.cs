
using System.Diagnostics;

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

    Puzzle ParsePuzzleTwo(string s)
    {
        var parts = s.Split(' ');
        return new Puzzle(RepeatFive(parts[0].Trim(), '?'), RepeatFive(parts[1], ',').Ints().ToArray());
    }

    string RepeatFive(string s, char c) => s + c + s + c + s + c + s + c + s;


    public override Answer One(string input)
    {
        var sw = Stopwatch.StartNew();
        int t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzle).ToList();

        foreach (var p in puzzles)
        {
            var c = CountValid(p.Springs, p.Lengths, 0);
            LogEx($"{p.Springs} count {c}");
            t += c;
        }

        Log($"Complete in {sw.ElapsedMilliseconds:0}");

        return t;
    }

    string ReplaceAt(string s, int i, char r)
    {
        var chars = s.ToCharArray();
        chars[i] = r;
        return new string(chars);
    }


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

    private int CountValid(string springs, int[] lengths, int i)
    {
        if (i == springs.Length)
            return IsValid(springs, lengths) ? 1 : 0;

        if (springs[i] != '?')
            return CountValid(springs, lengths, i + 1);

        if (!IsPotential(springs, lengths))
            return 0;

        int t = 0;
        t += CountValid(ReplaceAt(springs, i, '.'), lengths, i + 1);
        t += CountValid(ReplaceAt(springs, i, '#'), lengths, i + 1);
        return t;
    }

    public override Answer Two(string input)
    {
        return 0;
        int t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzleTwo).ToList();

        foreach (var p in puzzles)
        {
            var c = CountValid(p.Springs, p.Lengths, 0);
            LogEx($"{p.Springs} count {c}");
            t += c;
        }

        return t;
    }
}
