using System.Net.WebSockets;

namespace AdventOfCode.Year2023;

public class Day19 : Solution
{
    public override string Example => @"";

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
            .Select(wf => wf.Split(new [] {'{', '}', ','}, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(p => p[0], p => p.Skip(1).Select(s => new Rule(s)).ToList());
        var parts = input.Lines().SkipUntil(IsBlank).Where(IsNotBlank)
            .Select(wf => wf.Split(new [] {'{', '}', ','}, StringSplitOptions.RemoveEmptyEntries))
            .Select(p => p.ToDictionary(p => p.Split('=')[0], p => p.Int()))
            .ToList();

        foreach (var part in parts)
        {
            var wfName = "in";

            while(wfName != "A" && wfName != "R")
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
        return 0;
    }
}
