using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Year2024;

public class Day15 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var parts = input.Lines().Segment(IsBlank).ToList();
        var g = Grid.ParseFixed(string.Join('\n', parts[0]), '#');
        var moves = parts[1].Select(l => l.Replace("&lt;", "<").Replace("&gt;", ">")).SelectMany(l => l.ToArray()).Where(c => "<>^v".Contains(c)).ToArray();

        var p = g.Cells.Single(c => c.Value == '@').Key;
        g[p] = '.';

        foreach (var m in moves)
        {
            var d = m switch
            {
                '<' => new Point(-1, 0),
                '>' => new Point(1, 0),
                '^' => new Point(0, -1),
                'v' => new Point(0, 1),
                _ => throw new Exception(),
            };

            var c = g[p.Add(d)];

            switch (c)
            {
                case '.':
                    p = p.Add(d);
                    break;
                case '#':
                    break;
                case 'O':
                    var gap = p.Add(d);
                    while (g[gap] != '#')
                    {
                        if (g[gap] == '.')
                        {
                            g[gap] = 'O';
                            p = p.Add(d);
                            g[p] = '.';
                            break;
                        }
                        else if (g[gap] != 'O')
                            throw new Exception();
                        gap = gap.Add(d);
                    }
                    break;
                default:
                    throw new Exception();
            }
        }

        return g.Cells.Where(c => c.Value == 'O').Sum(c => c.Key.Y * 100 + c.Key.X );
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
