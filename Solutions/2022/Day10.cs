namespace AdventOfCode.Year2022;

public class Day10 : IDay
{
    public (bool, string) One(string input)
    {
        int x = 1;
        int t = 1;
        var p = new int[1000];
        p[t] = 1;
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            if (line.StartsWith("addx "))
            {
                t++;
                p[t] = x;
                t++;
                x += line.Ints()[0];
                p[t] = x;
            }
            else
            {
                t++;
                p[t] = x;
            }
        }
        var a =
            p[20] * 20
            + p[60] * 60
            + p[100] * 100
            + p[140] * 140
            + p[180] * 180
            + p[220] * 220;
        return (false, a.ToString());

    }

    public (bool, string) Two(string input)
    {
        int x = 1;
        int t = 1;
        var p = new int[1000];
        p[t] = 1;
        var crt = "\n";
        foreach(var line in input.Lines().Where(IsNotBlank))
        {
            if (line.StartsWith("addx "))
            {
                crt += Pix(p, t);
                t++;
                p[t] = x;
                crt += Pix(p, t);
                t++;
                x += line.Ints()[0];
                p[t] = x;
            }
            else
            {
                crt += Pix(p, t);
                t++;
                p[t] = x;
            }
        }
        return (false, crt);
    }

    string Pix(int[] p, int t)
    {
        return (Abs(p[t] - (t-1)%40) <= 1 ? "#" : ".") + (t%40 == 0 ? "\n" : "");
    }
}
