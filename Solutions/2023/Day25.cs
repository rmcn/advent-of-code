namespace AdventOfCode.Year2023;

public class Day25 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        int t = 0;

        var graph = new Dictionary<string, List<string>>();
        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            var p = line.Split(": ");
            var edges = p[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!graph.ContainsKey(p[0]))
                graph.Add(p[0], new List<string>());

            foreach (var e in edges)
            {
                graph[p[0]].Add(e);

                if (!graph.ContainsKey(e))
                    graph.Add(e, new List<string>());

                graph[e].Add(p[0]);
            }
        }

        var topEdges = TopShortestPathEdges(graph);
        var remove = topEdges.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).First().Split('-');

        graph[remove[0]].Remove(remove[1]);
        graph[remove[1]].Remove(remove[0]);

        var topEdges2 = TopShortestPathEdges(graph);
        var remove2 = topEdges2.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).First().Split('-');

        graph[remove2[0]].Remove(remove2[1]);
        graph[remove2[1]].Remove(remove2[0]);

        var topEdges3 = TopShortestPathEdges(graph);
        var remove3 = topEdges3.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).First().Split('-');

        graph[remove3[0]].Remove(remove3[1]);
        graph[remove3[1]].Remove(remove3[0]);

        int reachable = Reachable(graph, graph.Keys.First());
        int notReachable = graph.Keys.Count - reachable;

        return reachable * notReachable;
    }

    private int Reachable(Dictionary<string, List<string>> graph, string start)
    {
        var seen = new HashSet<string>() { start };
        var frontier = new Queue<string>();
        frontier.Enqueue(start);
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            foreach (var next in graph[current])
            {
                if (!seen.Contains(next))
                {
                    seen.Add(next);
                    frontier.Enqueue(next);
                }
            }
        }

        return seen.Count;
    }

    private Dictionary<string, int> TopShortestPathEdges(Dictionary<string, List<string>> graph)
    {
        // For each pair, find shortest path
        var shortestPathEdges = new List<string>();
        foreach (var (start, end) in graph.Keys.Zip(graph.Keys.Skip(1)))
        {
            var path = ShortestPath(graph, start, end);
            foreach (var (a, b) in path.Zip(path.Skip(1)))
            {
                var edge = string.Compare(a, b) < 0 ? $"{a}-{b}" : $"{b}-{a}";
                shortestPathEdges.Add(edge);
            }
        }
        // find the most common edges in those paths
        return shortestPathEdges.GroupBy(s => s).ToDictionary(g => g.Key, g => g.Count());
    }

    private List<string> ShortestPath(Dictionary<string, List<string>> graph, string start, string end)
    {
        var frontier = new PriorityQueue<string, int>();
        frontier.Enqueue(start, 0);
        var cameFrom = new Dictionary<string, string?> { [start] = null };
        var costSoFar = new Dictionary<string, int> { [start] = 0 };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == end)
                break;

            foreach (var next in graph[current])
            {
                var newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, newCost);
                    cameFrom[next] = current;
                }
            }
        }

        var path = new List<string>();

        if (!cameFrom.ContainsKey(end))
            return path;

        var curr = end;
        while (curr != start)
        {
            path.Add(curr);
            curr = cameFrom[curr];
        }
        path.Add(curr);
        path.Reverse();
        return path;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
