namespace AdventOfCode.Year2023;

using CatBounds = Dictionary<string, (int Lower, int Upper)>;

public class Day19 : Solution
{
    class Rule
    {
        public string Prop { get; set; }
        public string Op { get; set; }
        public int Value { get; set; }
        public string Dest { get; set; }
        public string Desc { get; set; }

        public Rule(string s)
        {
            Desc = s;
            var p = s.Split(':');
            if (p.Length == 1)
            {
                Dest = s;
                Op = "";
            }
            else
            {
                Prop = p[0][..1];
                Op = p[0].Substring(1, 1);
                Value = p[0].Int();
                Dest = p[1];
            }
        }

        public string? Next(Dictionary<string, int> part)
        {
            switch (Op)
            {
                case ">":
                    return (part[Prop] > Value) ? Dest : null;
                case "<":
                    return (part[Prop] < Value) ? Dest : null;
                case "":
                    return Dest;
                default:
                    throw new Exception(Desc);
            }
        }
    }


    public override Answer One(string input)
    {
        int t = 0;

        var wfs = input.Lines().TakeUntil(IsBlank).Where(IsNotBlank)
            .Select(wf => wf.Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(p => p[0], p => p.Skip(1).Select(s => new Rule(s)).ToList());
        var parts = input.Lines().SkipUntil(IsBlank).Where(IsNotBlank)
            .Select(wf => wf.Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(p => p.ToDictionary(p => p.Split('=')[0], p => p.Int()))
            .ToList();

        foreach (var part in parts)
        {
            var wfName = "in";

            while (wfName != "A" && wfName != "R")
            {
                var rules = wfs[wfName];
                wfName = rules.Select(r => r.Next(part)).First(d => d != null);
            }

            if (wfName == "A")
                t += part.Values.Sum();
        }

        return t;
    }

    public override Answer Two(string input)
    {
        var wfs = input.Lines().TakeUntil(IsBlank).Where(IsNotBlank)
            .Select(wf => wf.Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(p => p[0], p => p.Skip(1).Select(s => new Rule(s)).ToList());

        var startBounds = "xmas".ToCharArray().ToDictionary(c => c.ToString(), c => (Lower: 1, Upper: 4000));

        var pool = new Queue<(CatBounds CatBounds, string WfName)>();

        pool.Enqueue((startBounds, "in"));

        var accepted = new List<CatBounds>();

        while (pool.Count > 0)
        {
            var (path, wfname) = pool.Dequeue();

            var wf = wfs[wfname];
            foreach (var rule in wf)
            {
                if (rule.Op == ">")
                {
                    var match = new CatBounds(path);
                    match[rule.Prop] = match[rule.Prop] with { Lower = rule.Value + 1 };

                    if (rule.Dest == "A")
                        accepted.Add(match);
                    else if (rule.Dest != "R")
                        pool.Enqueue((match, rule.Dest));

                    path[rule.Prop] = path[rule.Prop] with { Upper = rule.Value };
                }
                else if (rule.Op == "<")
                {
                    var match = new CatBounds(path);
                    match[rule.Prop] = match[rule.Prop] with { Upper = rule.Value - 1 };

                    if (rule.Dest == "A")
                        accepted.Add(match);
                    else if (rule.Dest != "R")
                        pool.Enqueue((match, rule.Dest));

                    path[rule.Prop] = path[rule.Prop] with { Lower = rule.Value };
                }
                else if (rule.Op == "")
                {
                    if (rule.Dest == "A")
                        accepted.Add(path);
                    else if (rule.Dest != "R")
                        pool.Enqueue((path, rule.Dest));
                }
                else
                    throw new Exception();
            }
        }

        return accepted
            .Select(p => p.Values.Select(bounds => (long)bounds.Upper - bounds.Lower + 1).Product())
            .Sum();
    }
}
