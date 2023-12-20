namespace AdventOfCode.Year2023;

public class Day20 : Solution
{
    public override string Example => @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output";

    enum PulseType { Low, High }

    class Module
    {
        public char Type { get; set; }
        public string Name { get; set; }
        public List<string> Outputs { get; set; }
        public bool IsOn { get; set; }
        public Dictionary<string, PulseType> Last {get; set;} = new();

        public List<Pulse> Process(Pulse p)
        {
            switch (Type)
            {
                case 'b':
                    return Outputs.Select(s => new Pulse(Name, s, p.Type)).ToList();
                case '%':
                    if (p.Type == PulseType.High)
                        return new List<Pulse>();
                    IsOn = !IsOn;
                    return Outputs.Select(s => new Pulse(Name, s, IsOn ? PulseType.High : PulseType.Low)).ToList();
                case '&':
                    Last[p.Source] = p.Type;
                    var allHight = Last.Values.All(p => p == PulseType.High);
                    return Outputs.Select(s => new Pulse(Name, s, allHight ? PulseType.Low : PulseType.High)).ToList();
                default: throw new Exception();
            }
        }
    }

    Module ParseModule(string s)
    {
        var p = s.Split(" -> ");
        return new Module
        {
            Type = p[0][0],
            Name = p[0][0] == 'b' ? p[0] : p[0].Substring(1),
            Outputs = p[1].Split(',').Select(o => o.Trim()).ToList()
        };
    }

    record Pulse(string Source, string Dest, PulseType Type);

    public override Answer One(string input)
    {
        int t = 0;

        var modules = input.Lines().Where(IsNotBlank).Select(ParseModule).ToDictionary(m => m.Name, m => m);

        foreach (var mod in modules.Values)
        {
            var inputs = modules.Select(kvp => kvp.Value).Where(v => v.Outputs.Contains(mod.Name)).Select(v => v.Name).ToList();
            mod.Last = inputs.ToDictionary(k => k, k => PulseType.Low);
        }

        var pulses = new Queue<Pulse>();

        long highCount = 0;
        long lowCount = 0;

        for (int i =0; i < 1000; i++)
        {
            pulses.Enqueue(new Pulse("button", "broadcaster", PulseType.Low));

            while (pulses.Count > 0)
            {
                var pulse = pulses.Dequeue();

                if (pulse.Type == PulseType.High)
                    highCount++;
                else
                    lowCount++;

                if (modules.ContainsKey(pulse.Dest))
                {
                    var results = modules[pulse.Dest].Process(pulse);
                    foreach(var result in results)
                    {
                        pulses.Enqueue(result);
                    }
                }
            }
        }

        return highCount * lowCount;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
