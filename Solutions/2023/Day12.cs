
using System.Diagnostics;
using System.Text;

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
            var c = CountValid(new StringBuilder(p.Springs), p.Lengths, 0);
            LogEx($"{p.Springs} count {c}");
            t += c;
        }

        Log($"Complete in {sw.ElapsedMilliseconds:0}");

        return t;
    }

    StringBuilder ReplaceAt(StringBuilder s, int i, char r)
    {
        s[i] = r;
        return s;
    }


    bool IsValid(StringBuilder s, int[] lengths)
    {
        var j = 0;

        var brokenCount = 0;
        for (int i = 0; i <= s.Length; i++)
        {
            var c = i == s.Length ? '.' : s[i];
            if (c == '.')
            {
                if (brokenCount != 0)
                {
                    if (j >= lengths.Length || lengths[j] != brokenCount)
                        return false;
                    
                    j++;
                    brokenCount = 0;
                }
            }
            else
            {
                brokenCount++;
            }
        }

        return j == lengths.Length;
    }

    bool IsPotential(StringBuilder s, int[] lengths)
    {
        var broken = 0;
        var unknown = 0;
        var total = lengths.Sum();

        for(int i = 0; i < s.Length; i++)
        {
            var c = s[i];

            if (c == '#') broken++;
            if (c == '?') unknown++;
        }

        return broken <= total && broken + unknown >= total;
    }

    private int CountValid(StringBuilder springs, int[] lengths, int i)
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
        ReplaceAt(springs, i, '?');
        return t;
    }

    public override Answer Two(string input)
    {
        return 0;
        int t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzleTwo).ToList();

        foreach (var p in puzzles)
        {
            var c = CountValid(new StringBuilder(p.Springs), p.Lengths, 0);
            LogEx($"{p.Springs} count {c}");
            t += c;
        }

        return t;
    }
}
