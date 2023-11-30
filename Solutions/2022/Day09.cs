﻿using System.Drawing;

namespace AdventOfCode.Year2022;

public class Day09 : IDay
{
    public (bool, string) One(string input)
    {
        var h = new Point(0, 0);
        var t = new Point(0, 0);

        var locs = new HashSet<string>();
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            var dir = line[0];
            var dist = line.Ints()[0];
            var (dx, dy) = dir switch
            {
                'R' => (1, 0),
                'U' => (0, 1),
                'L' => (-1, 0),
                'D' => (0, -1)
            };

            for(int i = 0; i < dist; i++)
            {
                h.X += dx;
                h.Y += dy;

                var mx = h.X - t.X;
                var my = h.Y - t.Y;

                if (Abs(mx) <= 1 && Abs(my) <= 1)
                {
                    // Nothing
                }
                else if (Abs(mx) == 2 && my == 0)
                {
                    t.X += Sign(mx);
                }
                else if (mx == 0 && Abs(my) == 2)
                {
                    t.Y += Sign(my);
                }
                else if (Abs(mx) == 2 && Abs(my) == 1)
                {
                    t.X += Sign(mx);
                    t.Y += Sign(my);
                }
                else if (Abs(mx) == 1 && Abs(my) == 2)
                {
                    t.X += Sign(mx);
                    t.Y += Sign(my);
                }
                else
                {
                    throw new Exception($"{mx} {my}");
                }
                var loc = $"{t.X}, {t.Y}";

                if (!locs.Contains(loc))
                    locs.Add(loc);
            }
        }
        return (true, locs.Count.ToString());
    }

    public (bool, string) Two(string input)
    {
        var r = new Point[10];

        var locs = new HashSet<string>();
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            var dir = line[0];
            var dist = line.Ints()[0];
            var (dx, dy) = dir switch
            {
                'R' => (1, 0),
                'U' => (0, 1),
                'L' => (-1, 0),
                'D' => (0, -1)
            };

            for(int i = 0; i < dist; i++)
            {
                r[0].X += dx;
                r[0].Y += dy;

                for(int h = 0; h < r.Length-1; h++)
                {
                    int t = h + 1;
                    var mx = r[h].X - r[t].X;
                    var my = r[h].Y - r[t].Y;

                    if (Abs(mx) <= 1 && Abs(my) <= 1)
                    {
                        // Nothing
                    }
                    else if (Abs(mx) == 2 && my == 0)
                    {
                        r[t].X += Sign(mx);
                    }
                    else if (mx == 0 && Abs(my) == 2)
                    {
                        r[t].Y += Sign(my);
                    }
                    else if (Abs(mx) == 2 && Abs(my) == 1)
                    {
                        r[t].X += Sign(mx);
                        r[t].Y += Sign(my);
                    }
                    else if (Abs(mx) == 1 && Abs(my) == 2)
                    {
                        r[t].X += Sign(mx);
                        r[t].Y += Sign(my);
                    }
                    else if (Abs(mx) == 2 && Abs(my) == 2)
                    {
                        r[t].X += Sign(mx);
                        r[t].Y += Sign(my);
                    }
                    else
                    {
                        throw new Exception($"{mx} {my}");
                    }
                }
                var loc = $"{r[9].X}, {r[9].Y}";

                if (!locs.Contains(loc))
                    locs.Add(loc);
            }
        }
        return (false, locs.Count.ToString());
    }
}