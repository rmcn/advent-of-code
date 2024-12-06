namespace AdventOfCode.Year2024;

public class Day01 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var l1 = input.Ints().TakeEvery(2).Order();
        var l2 = input.Ints().Skip(1).TakeEvery(2).Order();

        return l1.Zip(l2).Sum(p => Abs(p.First - p.Second));
    }

    public override Answer Two(string input)
    {
        var l1 = input.Ints().TakeEvery(2);
        var d = input.Ints().Skip(1).TakeEvery(2).CountBy(x => x).ToDictionary();

        return l1.Sum(v => v * d.GetValueOrDefault(v, 0));
    }
}
