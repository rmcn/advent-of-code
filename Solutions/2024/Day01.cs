namespace AdventOfCode.Year2024;

public class Day01 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        int t = 0;

        var k = input.Lines().Where(IsNotBlank).Select((s, i) => s.Ints()).ToList();

        var l1 = k.Select(p => p[0]).OrderBy(v => v).ToList();
        var l2 = k.Select(p => p[1]).OrderBy(v => v).ToList();

        for (int i = 0; i < l1.Count; i++)
        {
            t += Abs(l1[i] - l2[i]);
        }

        return t;
    }

    public override Answer Two(string input)
    {
        int t = 0;

        var k = input.Lines().Where(IsNotBlank).Select((s, i) => s.Ints()).ToList();

        var l1 = k.Select(p => p[0]).ToList();
        var l2 = k.Select(p => p[1]).ToList();

        var d = l2.GroupBy(v => v).ToDictionary(v => v.Key, v => v.Count());

        foreach (var v in l1)
        {
            if (d.ContainsKey(v))
            {
                t += v * d[v];
            }
        }

        return t;  
    }
}
