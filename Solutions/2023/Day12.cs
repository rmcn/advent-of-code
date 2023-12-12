﻿
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
        var sw = Stopwatch.StartNew();
        ulong t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzle).ToList();

        foreach (var p in puzzles)
        {
            var c = CountValid(p.Springs, p.Lengths, 0, 0);
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

    private string ReplaceRange(string s, int start, int brokenCount)
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

    private ulong CountValid(string springs, int[] lengths, int i, int b)
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
            for (int j = i; j < springs.Length; j++)
                if (springs[j] == '#')
                    return 0;
            return 1;
        }

        if (i == springs.Length)
            return 0;

        if (springs[i] == '.')
        {
            return CountValid(springs, lengths, i + 1, b);
        }

        if (springs[i] == '#')
        {
            if (b >= lengths.Length)
                return 0;

            if (CanPlace(springs, i, lengths[b]))
            {
                var withRange = ReplaceRange(springs, i, lengths[b]);
                return CountValid(withRange, lengths, i + lengths[b] + 1, b + 1);
            }
            else
            {
                return 0;
            }
        }


        ulong t = 0;

        if (b < lengths.Length && CanPlace(springs, i, lengths[b]))
        {
            var withRange = ReplaceRange(springs, i, lengths[b]);
            t += CountValid(withRange, lengths, i + lengths[b] + 1, b + 1);
        }

        var withSpring = ReplaceAt(springs, i, '.');
        t += CountValid(withSpring, lengths, i + 1, b);

        return t;
    }


    // Can place a sequence of broken springs and a final '.' (or end of list)
    private bool CanPlace(string springs, int start, int brokenCount)
    {
        if (start + brokenCount > springs.Length)
            return false;

        for (int i = start; i < start + brokenCount; i++)
        {
            if (springs[i] == '.')
                return false;
        }

        if (start + brokenCount == springs.Length) // At end of string, no need for '.' terminator
            return true;

        // must end with a '.' or a '?'
        return springs[start + brokenCount] != '#';
    }


    static Dictionary<string, ulong> Lookup = null;

    public override Answer Two(string input)
    {
        ulong t = 0;

        var puzzles = input.Lines().Where(IsNotBlank).Select(ParsePuzzleTwo).ToList();

        foreach (var p in puzzles)
        {
            Log($"Processing {p.Springs} {string.Join(',', p.Lengths)}");

            Lookup = new Dictionary<string, ulong>();

            foreach (int i in GetHalfIndexes(p.Springs))
            {
                var half = p.Springs.Substring(i);
                for(int b = 0; b < p.Lengths.Length; b++)
                {
                    var x = CountValid(half, p.Lengths, 0, b);
                    Lookup.Add($"{half}:{b}", x);
                    //Log($"Half count at {i} for {b} is {x}");
                }
            }

            var c = CountValid(p.Springs, p.Lengths, 0, 0);
            t += c;
        }

        return t;
    }

    private List<int> GetHalfIndexes(string springs)
    {
        var result = new List<int>();
        for (int i = springs.Length / 5; i < springs.Length; i++)
        {
            if (springs[i] == '?' && (springs[i-1] == '?' || springs[i-1] == '.'))
                result.Add(i);
        }
        result.Reverse();
        return result;
    }
}