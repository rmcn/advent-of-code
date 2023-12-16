﻿using System.Drawing;

namespace AdventOfCode.Year2023;

public enum Dir { Up, Down, Left, Right }

public record Ray(Point Loc, Dir Dir);

public class Day16 : Solution
{
    public override Answer One(string input)
    {
        var g = ParseGrid(input);
        var startRay = new Ray(new Point(0, 0), Dir.Right);
        return CountEnegized(g, startRay);
    }

    public override Answer Two(string input)
    {
        var g = ParseGrid(input);
        var rows = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var width = rows[0].Length;
        var height = rows.Length;

        var rightRays = Enumerable.Range(0, height).Select(y => new Ray(new Point(0, y), Dir.Right));
        var leftRays = Enumerable.Range(0, height).Select(y => new Ray(new Point(width - 1, y), Dir.Left));
        var downRays = Enumerable.Range(0, width).Select(x => new Ray(new Point(x, 0), Dir.Down));
        var upRays = Enumerable.Range(0, width).Select(x => new Ray(new Point(x, height - 1), Dir.Up));

        var startRays = rightRays.Concat(leftRays).Concat(downRays).Concat(upRays);

        return startRays.Select(r => CountEnegized(g, r)).Max();
    }

    private static int CountEnegized(Dictionary<Point, char> g, Ray startRay)
    {
        var waveFront = new Queue<Ray>(new[] { startRay });
        var seen = new HashSet<Ray>();

        while (waveFront.Count > 0)
        {
            var ray = waveFront.Dequeue();
            var c = g.GetValueOrDefault(ray.Loc, '#');

            if (c == '#' || seen.Contains(ray))
                continue;

            seen.Add(ray);

            switch (c)
            {
                case '.':
                    waveFront.Enqueue(ray.Next());
                    break;
                case '|':
                    if (ray.Dir == Dir.Left || ray.Dir == Dir.Right)
                    {
                        waveFront.Enqueue(ray.Up());
                        waveFront.Enqueue(ray.Down());
                    }
                    else
                    {
                        waveFront.Enqueue(ray.Next());
                    }
                    break;
                case '-':
                    if (ray.Dir == Dir.Up || ray.Dir == Dir.Down)
                    {
                        waveFront.Enqueue(ray.Left());
                        waveFront.Enqueue(ray.Right());
                    }
                    else
                    {
                        waveFront.Enqueue(ray.Next());
                    }
                    break;
                case '\\':
                    if (ray.Dir == Dir.Up)
                        waveFront.Enqueue(ray.Left());
                    if (ray.Dir == Dir.Down)
                        waveFront.Enqueue(ray.Right());
                    if (ray.Dir == Dir.Left)
                        waveFront.Enqueue(ray.Up());
                    if (ray.Dir == Dir.Right)
                        waveFront.Enqueue(ray.Down());
                    break;
                case '/':
                    if (ray.Dir == Dir.Up)
                        waveFront.Enqueue(ray.Right());
                    if (ray.Dir == Dir.Down)
                        waveFront.Enqueue(ray.Left());
                    if (ray.Dir == Dir.Left)
                        waveFront.Enqueue(ray.Down());
                    if (ray.Dir == Dir.Right)
                        waveFront.Enqueue(ray.Up());
                    break;
                default:
                    throw new Exception();
            }
        }

        return seen.Select(r => r.Loc).Distinct().Count();
    }

    private static Dictionary<Point, char> ParseGrid(string input)
    {
        var g = new Dictionary<Point, char>();

        foreach (var (s, y) in input.Lines().Where(IsNotBlank).Select((s, y) => (s, y)))
        {
            foreach (var (c, x) in s.ToCharArray().Select((c, x) => (c, x)))
            {
                g[new Point(x, y)] = c;
            }
        }

        return g;
    }
}

public static class Day16Extensions
{
    public static Ray Next(this Ray ray)
    {
        return ray.Dir switch
        {
            Dir.Up => ray.Up(),
            Dir.Down => ray.Down(),
            Dir.Left => ray.Left(),
            Dir.Right => ray.Right(),
            _ => throw new Exception(),
        };
    }

    public static Ray Up(this Ray ray)
        => ray with { Loc = ray.Loc with { Y = ray.Loc.Y - 1 }, Dir = Dir.Up };
    public static Ray Down(this Ray ray)
        => ray with { Loc = ray.Loc with { Y = ray.Loc.Y + 1 }, Dir = Dir.Down };
    public static Ray Left(this Ray ray)
        => ray with { Loc = ray.Loc with { X = ray.Loc.X - 1 }, Dir = Dir.Left };
    public static Ray Right(this Ray ray)
        => ray with { Loc = ray.Loc with { X = ray.Loc.X + 1 }, Dir = Dir.Right };
}
