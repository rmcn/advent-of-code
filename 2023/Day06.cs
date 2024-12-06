namespace AdventOfCode.Year2023;

public class Day06 : Solution
{
    public override Answer One(string input)
    {
        ulong t = 1;

        var lines = input.Lines().Where(IsNotBlank).ToList();
        var times = lines[0].Ints();
        var dists = lines[1].Ints();

        for (int i = 0; i < times.Count; i++)
        {
            var time = times[i];
            var dist = dists[i];

            ulong ways = 0;
            for (int pressFor = 0; pressFor < time; pressFor++)
            {
                var timeLeft = time - pressFor;
                var travel = timeLeft * pressFor;
                if (travel > dist)
                    ways++;
            }

            t *= ways;
        }

        return t;
    }

    public override Answer Two(string input)
    {
        var lines = input.Lines().Where(IsNotBlank).ToList();
        var time = lines[0].Replace(" ", "").Long();
        var dist = lines[1].Replace(" ", "").Long();

        ulong ways = 0;
        for (int pressFor = 0; pressFor < time; pressFor++)
        {
            var timeLeft = time - pressFor;
            var travel = timeLeft * pressFor;
            if (travel > dist)
                ways++;
        }
        return ways;
    }
}
