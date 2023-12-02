﻿
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023;

public class Day02 : Solution
{
    public override object One(string input)
    {
        int t = 0;

        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            var game = line.Split(":");
            var rounds = game[1].Split(";");

            if (rounds.All(IsPossible))
            {
                t += game[0].Ints()[0];
            }
        }

        return t;
    }

    private bool IsPossible(string round)
    {
        var colours = round.Split(",");
        foreach(var colour in colours)
        {
            if (colour.EndsWith("blue")) {
                if (colour.Ints()[0] > 14) return false;
            } else if (colour.EndsWith("red")) {
                if (colour.Ints()[0] > 12) return false;
            } else if (colour.EndsWith("green")) {
                if (colour.Ints()[0] > 13) return false;
            } else
                throw new Exception($"[{colour}]");
        }
        return true;
    }

    public override object Two(string input)
    {
        int t = 0;

        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            var game = line.Split(":");
            var rounds = game[1].Split(";");
            t += Power(rounds);
        }

        return t;
    }

    private int Power(string[] rounds)
    {
        var r = 0;
        var g = 0;
        var b = 0;
        foreach(var round in rounds)
        {
            var colours = round.Split(",");
            foreach(var colour in colours)
            {
                if (colour.EndsWith("blue")) {
                    b = Max(b, colour.Ints()[0]);
                } else if (colour.EndsWith("red")) {
                    r = Max(r, colour.Ints()[0]);
                } else if (colour.EndsWith("green")) {
                    g = Max(g, colour.Ints()[0]);
                } else
                    throw new Exception($"[{colour}]");
            }
        }

        return r * g * b;
    }
}
