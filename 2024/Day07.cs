

namespace AdventOfCode.Year2024;

public class Day07 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        return  input
            .Lines()
            .Where(IsNotBlank)
            .Select(l => l.Longs())
            .Where(l => IsValid(l, Op.Add, Op.Mul))
            .Sum(i => i.First());
    }

    private bool IsValid(List<long> nums, params Op[] ops)
    {
        var target = nums.First();
        var parts = nums.Skip(1).ToList();
        return ops.Any(op => TryValid(target, parts, parts[0], 1, op, ops));
    }

    private bool TryValid(long target, List<long> parts, long current, int i, Op op, Op[] ops)
    {
        current = op switch
        {
            Op.Add => current + parts[i],
            Op.Mul => current * parts[i],
            Op.Concat => current * (long)Math.Pow(10, parts[i].ToString().Length) + parts[i],
            _ => throw new Exception()
        };

        if (i == parts.Count - 1)
        {
            return target == current;
        }

        return ops.Any(op => TryValid(target, parts, current, i+1, op, ops));
    }

    public override Answer Two(string input)
    {
        return  input
            .Lines()
            .Where(IsNotBlank)
            .Select(l => l.Longs())
            .Where(l => IsValid(l, Op.Add, Op.Mul, Op.Concat))
            .Sum(i => i.First());
    }

    enum Op
    {
        Add,
        Mul,
        Concat
    }
}
