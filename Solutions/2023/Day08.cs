namespace AdventOfCode.Year2023;

public class Day08 : Solution
{
    public override Answer One(string input)
    {
        int t = 0;

        var directions = input.Lines().Where(IsNotBlank).First().Trim();

        var nodes = input.Lines().Where(IsNotBlank).Skip(1).Select(ParseNode).ToDictionary(n => n.Loc);

        var current = nodes["AAA"];

        while(true)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                current = directions[i] == 'L' ? nodes[current.Left] : nodes[current.Right];
                t++;

                if (current.Loc == "ZZZ")
                    return t;
            }
        }
    }

    record Node(string Loc, string Left, string Right);

    Node ParseNode(string node)
    {
        var parts = node.Split(new char[] {'=', ' ', '(', ')', ','}, StringSplitOptions.RemoveEmptyEntries);
        return new Node(parts[0], parts[1], parts[2]);
    }

    public ulong CommonMultiple(ulong[] periods)
    {        
        ulong x = 0;
        ulong step = periods.Max();
        while(true)
        {
            int matchCount = 0;
            for (int l = 0; l < periods.Length; l++)
            {
                if (x % periods[l] == 0 )
                    matchCount++;
            }

            if (matchCount >= periods.Length - 1)
                Log($"{matchCount} match at {x}");
            
            if (matchCount == periods.Length && x != 0)
                return x;

            x += step;
        }
    }
    public override Answer Two(string input)
    {
        var directions = input.Lines().Where(IsNotBlank).First().Trim();

        var nodes = input.Lines().Where(IsNotBlank).Skip(1).Select(ParseNode).ToDictionary(n => n.Loc);

        var ghostStarts = nodes.Values.Where(n => n.Loc.EndsWith("A")).ToList();
        var periods = ghostStarts.Select(g => GhostPeriod(directions, nodes, g)).ToArray();

        return CommonMultiple(periods);
    }

    private ulong GhostPeriod(string directions, Dictionary<string, Node> nodes, Node ghost)
    {
        var start = ghost.Loc;
        var dist = 0;
        var zCount = 0;
        var lastZ = 0;
        while (true)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                ghost = directions[i] == 'L' ? nodes[ghost.Left] : directions[i] == 'R' ? nodes[ghost.Right] : throw new Exception();
                dist++;
                if (ghost.Loc.EndsWith("Z"))
                {
                    if (lastZ == 0)
                        Log($"Start {start} {dist}");
                    else
                        Log($"Start {start} {dist} {dist - lastZ}");

                    if (zCount++ >= 3)
                    {
                        return (ulong)(dist - lastZ);
                    }

                    lastZ = dist;
                }
            }
        }
    }
}
