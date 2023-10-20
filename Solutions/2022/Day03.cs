namespace AdventOfCode.Year2022;

class Day03 : IDay
{
    public int Year => 2022;
    public int Day => 3;

    public (bool, string) One(string input) 
    {
        var t = 0;

        foreach (var line in  input.Lines().Where(IsNotBlank))
        {
            var a = line.Substring(0, line.Length / 2);
            var b = line.Substring(line.Length / 2, line.Length / 2);

            var e = a.Except(a.Except(b.ToCharArray())).ToList().Single();

            if (e.ToString().ToLower() == e.ToString())
            {
                t += e - 'a' + 1;
            }
            else
            {
                t += e - 'A' + 27;
            }
        }

        return (true, t.ToString());
    }

    public (bool, string) Two(string input)
    {
        var t = 0;

        foreach (var batch in input.Lines().Where(IsNotBlank).Batch(3))
        {
            var a = batch.ToList()[0];
            var b = batch.ToList()[1];
            var c = batch.ToList()[2];

            var ab = a.Except(a.Except(b.ToCharArray())).ToList();
            var e = ab.Except(ab.Except(c.ToCharArray())).Single();

            if (e.ToString().ToLower() == e.ToString())
            {
                t += e - 'a' + 1;
            }
            else
            {
                t += e - 'A' + 27;

            }
        }

        return (true, t.ToString());
    }
}