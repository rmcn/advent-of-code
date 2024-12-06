namespace AdventOfCode.Year2020;

public class Day07 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var bags = input.Lines().Where(IsNotBlank).Select(l => ParseBag(l)).ToDictionary();

        return bags.Keys.Count(b => ContainsGold(b, bags));
    }

    private bool ContainsGold(string bag, Dictionary<string, List<(int, string)>> bags)
    {
        var contents = bags[bag].Select(b => b.Item2);
        if (contents.Contains("shiny gold" ))
            return true;

        return contents.Any(b => ContainsGold(b, bags));
    }

    private (string key, List<(int, string)> bags) ParseBag(string line)
    {
        var s = line.Replace("bags.", "").Replace("bags", "").Replace("bag.", "").Replace("bag", "");
        var parts = s.Split("contain");
        return (parts[0].Trim(), parts[1].Split(",").Select(b => b.Trim()).Where(b => b != "no other").Select(ParseBagCount).ToList());
    }

    private (int, string) ParseBagCount(string bagCount)
    {
        return (bagCount.Int(), string.Join(" ", bagCount.Split(" ").Skip(1)));
    }

    public override Answer Two(string input)
    {
        var bags = input.Lines().Where(IsNotBlank).Select(l => ParseBag(l)).ToDictionary();

        return CountBagAndContents("shiny gold", bags) - 1; // Take away the shiny gold bag
    }

    private long CountBagAndContents(string bag, Dictionary<string, List<(int, string)>> bags)
    {
        return 1 + bags[bag].Sum(b => b.Item1 * CountBagAndContents(b.Item2, bags));
    }
}