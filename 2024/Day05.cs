


namespace AdventOfCode.Year2024;

public class Day05 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var parts = input.Lines().Segment(IsBlank).ToList();

        var orderings = parts[0].Select(l => (first: l.Ints()[0], second: l.Ints()[1])).ToList();
        var updates = parts[1].Where(IsNotBlank).Select(l => l.Ints());

        return updates.Where(u => IsValid(u, orderings)).Sum(u => Middle(u));
    }

    private decimal Middle(List<int> u)
    {
        if (u.Count % 2 == 0) throw new Exception("even");
        return u[(u.Count-1) / 2];
    }

    private bool IsValid(List<int> u, List<(int first, int second)> orderings)
    {
        foreach (var (first, second) in orderings)
        {
            var iFirst = u.IndexOf(first);
            var iSecond = u.IndexOf(second);

            if (iFirst != -1 && iSecond != -1 && iFirst > iSecond)
                return false;
        }
        return true;
    }

    public override Answer Two(string input)
    {
        var parts = input.Lines().Segment(IsBlank).ToList();

        var orderings = parts[0].Select(l => (first: l.Ints()[0], second: l.Ints()[1])).ToList();
        var updates = parts[1].Where(IsNotBlank).Select(l => l.Ints());

        return updates.Where(u => !IsValid(u, orderings)).Select(u => Reorder(u, orderings)).Sum(u => Middle(u));
    }

    private List<int> Reorder(List<int> u, List<(int first, int second)> orderings)
    {
        while(!IsValid(u, orderings))
        {
            foreach (var (first, second) in orderings)
            {
                var iFirst = u.IndexOf(first);
                var iSecond = u.IndexOf(second);

                if (iFirst != -1 && iSecond != -1 && iFirst > iSecond)
                {
                    var t = u[iFirst];
                    u[iFirst] = u[iSecond];
                    u[iSecond] = t;
                }
            }
        }
        return u;
    }
}
