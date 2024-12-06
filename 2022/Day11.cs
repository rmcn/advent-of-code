namespace AdventOfCode.Year2022;

public class Day11 : Solution
{
    Monkey[] ExampleMonkies = new[]
    {
        new Monkey { Index = 0, Op = (old) => old * 19, Test = new (23, 2, 3), InitItems = {79, 98} },
        new Monkey { Index = 1, Op = (old) => old + 6, Test = new (19, 2, 0), InitItems = {54, 65, 75, 74} },
        new Monkey { Index = 2, Op = (old) => old * old, Test = new(13, 1, 3), InitItems = {79, 60, 97} },
        new Monkey { Index = 3, Op = (old) => old + 3, Test = new (17, 0, 1), InitItems = {74} },
    };

    Monkey[] InputMonkies = new[]
    {
        new Monkey { Index = 0, Op = (old) => old * 19,  Test = new (7, 6, 2), InitItems = {59, 74, 65, 86} },
        new Monkey { Index = 1, Op = (old) => old + 1,   Test = new (2, 2, 0), InitItems = {62, 84, 72, 91, 68, 78, 51} },
        new Monkey { Index = 2, Op = (old) => old + 8,   Test = new (19, 6, 5), InitItems = {78, 84, 96} },
        new Monkey { Index = 3, Op = (old) => old * old, Test = new (3, 1, 0), InitItems = {97, 86} },
        new Monkey { Index = 4, Op = (old) => old + 6,   Test = new (13, 3, 1), InitItems = {50} },
        new Monkey { Index = 5, Op = (old) => old * 17,  Test = new (11, 4, 7), InitItems = {73, 65, 69, 65, 51} },
        new Monkey { Index = 6, Op = (old) => old + 5,   Test = new (5, 5, 7), InitItems = {69, 82, 97, 93, 82, 84, 58, 63} },
        new Monkey { Index = 7, Op = (old) => old + 3,   Test = new (17, 3, 4), InitItems = {81, 78, 82, 76, 79, 80} },
    };

    public override Answer One(string input)
    {
        var monkies = InputMonkies;

        foreach (var monkey in monkies)
            monkey.Reset();

        for (int round = 0; round < 20; round++)
        {
            foreach (var monkey in monkies)
            {
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.Items[0];
                    monkey.Items.RemoveAt(0);
                    item = (ulong)Floor((decimal)monkey.Op(item) / 3);
                    var passTo = item % monkey.Test.Mod == 0 ? monkey.Test.IndexTrue : monkey.Test.IndexFalse;
                    monkies[passTo].Items.Add(item);
                    monkey.InspectCount++;
                }
            }
        }

        return monkies.OrderByDescending(m => m.InspectCount).Select(m => m.InspectCount).Take(2).Product();
    }

    public override Answer Two(string input)
    {
        return 0;
    }

    record Test(ulong Mod, int IndexTrue, int IndexFalse);

    class Monkey
    {
        public int Index { get; set; }
        public ulong InspectCount { get; set; } = 0;
        public List<ulong> InitItems { get; set; } = new();
        public List<ulong> Items { get; set; } = new();
        public Func<ulong, ulong> Op = (ulong x) => x;
        public Test Test { get; set; } = new Test(0, 0, 0);

        public void Reset()
        {
            InspectCount = 0;
            Items = InitItems.ToList();
        }
    }
}
