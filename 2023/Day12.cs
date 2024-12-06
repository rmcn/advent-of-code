
using System.Diagnostics;

namespace AdventOfCode.Year2023;

public class Day12 : Solution
{
    public override string Example => @"";

    record Puzzle(string Springs, int[] Lengths);

    Puzzle ParsePuzzle(string s)
    {
        var parts = s.Split(' ');
        return new Puzzle(parts[0].Trim() + '.', parts[1].Ints().ToArray());
    }

    Puzzle ParsePuzzleTwo(string s)
    {
        var parts = s.Split(' ');
        return new Puzzle(RepeatFive(parts[0].Trim(), '?') + '.', RepeatFive(parts[1], ',').Ints().ToArray());
    }

    string RepeatFive(string s, char c) => s + c + s + c + s + c + s + c + s;

    public override Answer One(string input)
    {
        Lookup.Clear(); // Don't need memoization for part 1

        var sw = Stopwatch.StartNew();

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzle).ToList();

        ulong t = 0;
        foreach (var p in puzzles)
        {
            t += CountValid(p.Springs, p.Lengths, 0);
        }

        Log($"Complete in {sw.ElapsedMilliseconds:0}");

        return t;
    }


    ulong CountValid(string springs, int[] brokenRunLengths, int brokenRunIndex)
    {
        var key = springs + ":" + brokenRunIndex;
        if (Lookup.ContainsKey(key))
        {
            return Lookup[key];
        }

        if (brokenRunIndex == brokenRunLengths.Length)
        {
            // We've run out of broken runs to place
            // For this to be a valid placement all remaining springs should be '?' or '.'
            for (int i = 0; i < springs.Length; i++)
                if (springs[i] == '#')
                    return 0;
            return 1;
        }

        // If we've reached the end this is not a valid solution, because we haven't placed all our broken run yet
        if (springs == "")
            return 0;

        // Step over working springs
        if (springs[0] == '.')
        {
            return CountValid(springs.Substring(1), brokenRunLengths, brokenRunIndex);
        }

        if (springs[0] == '#')
        {
            // A broken spring run must start here
            if (CanPlaceBrokenRun(springs, brokenRunLengths[brokenRunIndex]))
            {
                return CountValid(springs.Substring(brokenRunLengths[brokenRunIndex] + 1), brokenRunLengths, brokenRunIndex + 1);
            }
            else
            {
                return 0;
            }
        }

        // Must be a '?' so try placing both '#' and '.'
        ulong t = 0;

        if (CanPlaceBrokenRun(springs, brokenRunLengths[brokenRunIndex]))
        {
            t += CountValid(springs.Substring(brokenRunLengths[brokenRunIndex] + 1), brokenRunLengths, brokenRunIndex + 1);
        }

        t += CountValid(springs.Substring(1), brokenRunLengths, brokenRunIndex);

        return t;
    }

    // Can we place a sequence of broken springs and a final '.' (or use ones already there)
    private bool CanPlaceBrokenRun(string springs, int brokenCount)
    {
        if (brokenCount > springs.Length)
            return false;

        for (int i = 0; i < brokenCount; i++)
        {
            if (springs[i] == '.')
                return false;
        }

        // must end with a '.' or a '?'
        return springs[brokenCount] != '#';
    }

    static Dictionary<string, ulong> Lookup = new();

    public override Answer Two(string input)
    {
        ulong t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzleTwo).ToList();

        foreach (var p in puzzles)
        {
            //Log($"Processing {p.Springs} {string.Join(',', p.Lengths)}");

            Lookup = new Dictionary<string, ulong>();

            // Break the list into smaller sub-problems, starting at the end and working back
            // Memoise the counts as we go, so longer sub-problems can benefit from answers we've
            // already calculated
            foreach (int i in StartIndexes(p.Springs))
            {
                var partialSprings = p.Springs.Substring(i);
                for (int b = 0; b < p.Lengths.Length; b++)
                {
                    var x = CountValid(partialSprings, p.Lengths, b);
                    Lookup.Add($"{partialSprings}:{b}", x);
                }
            }

            t += CountValid(p.Springs, p.Lengths, 0);
        }

        return t;
    }

    // Indexes where it's possible to place a broken run.
    private List<int> StartIndexes(string springs)
    {
        var result = new List<int>();
        for (int i = springs.Length / 5; i < springs.Length; i++)
        {
            if (springs[i] == '?' && (springs[i - 1] == '?' || springs[i - 1] == '.'))
                result.Add(i);
        }
        result.Reverse();
        return result;
    }
}
