using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day08 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var g = Grid.ParseFixed(input, '.');
        var g2 = Grid.ParseFixed(input, '.');

        var nodeTypes = g.Cells.Values.Distinct().Where(c => c != '.').ToList();

        var antiNodes = new HashSet<Point>();

        foreach (var nodeType in nodeTypes)
        {
            var nodes = g.Cells.Where(c => c.Value == nodeType).ToList();
            foreach (var n1 in nodes)
            {
                foreach (var n2 in nodes)
                {
                    if (n1.Key == n2.Key)
                        continue;

                    var dx = n1.Key.X - n2.Key.X;
                    var dy = n1.Key.Y - n2.Key.Y;

                    var an1 = new Point(n1.Key.X + dx, n1.Key.Y + dy);
                    var an2 = new Point(n2.Key.X - dx, n2.Key.Y - dy);

                    foreach (var an in new [] {an1, an2}.Where(n => g.InBounds(n)))
                        antiNodes.Add(an);

                    //  5   7   == -2 add first sub second
                    //  7   5   ==  2 add first sub second
                }
            }
        }

        foreach(var x in antiNodes) 
        {
            g2[x] = '#';
        }
        LogEx("");
        LogEx(g2.Dump());

        return antiNodes.Count();
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseFixed(input, '.');
        var g2 = Grid.ParseFixed(input, '.');

        var nodeTypes = g.Cells.Values.Distinct().Where(c => c != '.').ToList();

        var antiNodes = new HashSet<Point>();

        foreach (var nodeType in nodeTypes)
        {
            var nodes = g.Cells.Where(c => c.Value == nodeType).ToList();
            foreach (var n1 in nodes)
            {
                foreach (var n2 in nodes)
                {
                    if (n1.Key == n2.Key)
                        continue;

                    var dx = n1.Key.X - n2.Key.X;
                    var dy = n1.Key.Y - n2.Key.Y;

                    var an1 = n1.Key;
                    var an2 = n2.Key;

                    while (g.InBounds(an1) || g.InBounds(an2))
                    {
                        if (g.InBounds(an1))
                            antiNodes.Add(an1);

                        if (g.InBounds(an2))
                            antiNodes.Add(an2);

                        an1 = new Point(an1.X + dx, an1.Y + dy);
                        an2 = new Point(an2.X - dx, an2.Y - dy);

                    }

                    //  5   7   == -2 add first sub second
                    //  7   5   ==  2 add first sub second
                }
            }
        }

        foreach(var x in antiNodes) 
        {
            g2[x] = '#';
        }
        LogEx("");
        LogEx(g2.Dump());

        return antiNodes.Count();
    }
}
