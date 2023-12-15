using System.Text;

namespace AdventOfCode.Year2023;

public class Day15 : Solution
{
    public override string Example => @"";

    public override Answer One(string input) =>
        input
            .Replace("\n", "")
            .Split(',')
            .Select(Hash)
            .Sum();

    int Hash(string s) =>
        Encoding.ASCII.GetBytes(s)
            .Select(b => (int)b)
            .Aggregate(0, (a, b) => (a + b) * 17 % 256);

    record Lens(string Label, int FocalLength);

    public override Answer Two(string input)
    {
        var boxes = Enumerable.Range(0, 256).Select(i => new List<Lens>()).ToArray();

        foreach (var instruction in input.Replace("\n", "").Split(','))
        {
            if (instruction.EndsWith("-"))
            {
                var label = instruction[..^1];
                var i = Hash(label);
                boxes[i] = boxes[i].Where(l => l.Label != label).ToList();
            }
            else if (instruction.Contains("="))
            {
                var parts = instruction.Split("=");
                var lens = new Lens(parts[0], parts[1].Int());
                var i = Hash(lens.Label);

                if (boxes[i].Any(l => l.Label == lens.Label))
                    boxes[i] = boxes[i].Select(l => l.Label == lens.Label ? lens : l).ToList();
                else
                    boxes[i].Add(lens);
            }
            else
            {
                throw new Exception(instruction);
            }
        }

        return boxes.Select((list, i) => list.Select((lens, j) => (i + 1) * (j + 1) * lens.FocalLength).Sum()).Sum();
    }
}
