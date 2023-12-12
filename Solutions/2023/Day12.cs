
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
        Lookup = null; // Don't need memoization for part 1

        var sw = Stopwatch.StartNew();

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzle).ToList();

        ulong t = 0;
        foreach (var p in puzzles)
        {
            t += CountValid(p.Springs, p.Lengths, 0, 0);
        }

        Log($"Complete in {sw.ElapsedMilliseconds:0}");

        return t;
    }


    ulong CountValid(string springs, int[] lengths, int i, int b)
    {
        if (Lookup != null)
        {
            var key = springs.Substring(i) + ":" + b;
            if (Lookup.ContainsKey(key))
            {
                return Lookup[key];
            }
        }

        if (b == lengths.Length)
        {
            // We've run out of broken runs to place
            // For this to be a valid placement all remaining springs should be '?' or '.'
            for (int j = i; j < springs.Length; j++)
                if (springs[j] == '#')
                    return 0;
            return 1;
        }

        // If we've reached the end this is not a valid solution, because we haven't placed all our broken run yet
        if (i == springs.Length)
            return 0;

        // Step over working springs
        if (springs[i] == '.')
        {
            return CountValid(springs, lengths, i + 1, b);
        }

        if (springs[i] == '#')
        {
            // A broken spring run must start here
            if (CanPlaceBrokenRun(springs, i, lengths[b]))
            {
                var withBroken = PlaceBrokenRun(springs, i, lengths[b]);
                return CountValid(withBroken, lengths, i + lengths[b] + 1, b + 1);
            }
            else
            {
                return 0;
            }
        }

        // Must be a '?' so try placing both '#' and '.'
        ulong t = 0;

        if (CanPlaceBrokenRun(springs, i, lengths[b]))
        {
            var withRange = PlaceBrokenRun(springs, i, lengths[b]);
            t += CountValid(withRange, lengths, i + lengths[b] + 1, b + 1);
        }

        var withWorking = PlaceSingleWorking(springs, i);
        t += CountValid(withWorking, lengths, i + 1, b);

        return t;
    }

    // Can we place a sequence of broken springs and a final '.' (or use ones already there)
    private bool CanPlaceBrokenRun(string springs, int start, int brokenCount)
    {
        if (start + brokenCount > springs.Length)
            return false;

        for (int i = start; i < start + brokenCount; i++)
        {
            if (springs[i] == '.')
                return false;
        }

        // must end with a '.' or a '?'
        return springs[start + brokenCount] != '#';
    }

    string PlaceSingleWorking(string s, int i)
    {
        var chars = s.ToCharArray();
        chars[i] = '.';
        return new string(chars);
    }

    string PlaceBrokenRun(string s, int start, int brokenCount)
    {
        var chars = s.ToCharArray();
        for (int i = start; i < start + brokenCount; i++)
        {
            chars[i] = '#';
        }

        if (start + brokenCount < s.Length)
            chars[start + brokenCount] = '.';

        return new string(chars);
    }


    static Dictionary<string, ulong> Lookup = null;

    public override Answer Two(string input)
    {
        ulong t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzleTwo).ToList();

        foreach (var p in puzzles)
        {
            //Log($"Processing {p.Springs} {string.Join(',', p.Lengths)}");

            Lookup = new Dictionary<string, ulong>();

            // Break the list into smaller sub-problems, starting at the end and working back
            // Memoise the counts as we go, so longer sequences can benefit from answers we've
            // already calculated
            foreach (int i in StartIndexes(p.Springs))
            {
                var partialSprings = p.Springs.Substring(i);
                for (int b = 0; b < p.Lengths.Length; b++)
                {
                    var x = CountValid(partialSprings, p.Lengths, 0, b);
                    Lookup.Add($"{partialSprings}:{b}", x);
                }
            }

            t += CountValid(p.Springs, p.Lengths, 0, 0);
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
