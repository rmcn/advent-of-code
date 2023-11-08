namespace AdventOfCode.Year2022;

public class Day08 : IDay
{
    public (bool, string) One(string input)
    {
        int c = 0;
        var g = input.Lines().Where(IsNotBlank).Select(s => s.Trim()).ToArray();

        for (int x = 0; x < g.Length; x++)
        {
            for (int y = 0; y < g[x].Length; y++)
            {
                if(isVis(x, y, g))
                {
                    c++;
                }
            }
            
        }

        return (true, c.ToString());
    }

    private bool isVis(int x, int y, string[] g)
    {
        return isVisL(x, y, g) || isVisR(x, y, g)  || isVisT(x, y, g)  || isVisB(x, y, g);
    }

    private bool isVisL(int x, int y, string[] g)
    {
        var h = g[x][y];
        if (x == 0)
            return true;
        while(--x >= 0)
        {
            if (g[x][y] >= h)
                return false;
        }
        return true;
    }

    
    private bool isVisT(int x, int y, string[] g)
    {
        var h = g[x][y];
        if (y == 0)
            return true;
        while(--y >= 0)
        {
            if (g[x][y] >= h)
                return false;
        }
        return true;
    }

    private bool isVisR(int x, int y, string[] g)
    {
        var h = g[x][y];
        if (x == g.Length - 1)
            return true;
        while(++x <= g.Length - 1)
        {
            if (g[x][y] >= h)
                return false;
        }
        return true;
    }


    private bool isVisB(int x, int y, string[] g)
    {
        var h = g[x][y];
        if (y == g[x].Length - 1)
            return true;
        while(++y <= g[x].Length - 1)
        {
            if (g[x][y] >= h)
                return false;
        }
        return true;
    }

    public (bool, string) Two(string input)
    {
        int c = 0;
        var g = input.Lines().Where(IsNotBlank).Select(s => s.Trim()).ToArray();

        for (int x = 0; x < g.Length; x++)
        {
            for (int y = 0; y < g[x].Length; y++)
            {
                c = Math.Max(c, VL(x, y, g) * VU(x, y, g) * VR(x, y, g) * VD(x, y, g));
            }
        }

        return (true, c.ToString());

    }

    private int VL(int x, int y, string[] g)
    {
        var d = 0;
        var h = g[x][y];
        while(--x >= 0 && g[x][y] < h)
        {
            d++;
        }
        if (x >= 0) d++;
        return d;
    }
    private int VR(int x, int y, string[] g)
    {
        var d = 0;
        var h = g[x][y];
        while(++x <= g.Length - 1 && g[x][y] < h)
        {
            d++;
        }
        if (x <= g.Length - 1) d++;
        return d;
    }

    private int VU(int x, int y, string[] g)
    {
        var d = 0;
        var h = g[x][y];
        while(--y >= 0 && g[x][y] < h)
        {
            d++;
        }
        if (y >= 0) d++;
        return d;
    }
    private int VD(int x, int y, string[] g)
    {
        var d = 0;
        var h = g[x][y];
        while(++y <= g[x].Length - 1 && g[x][y] < h)
        {
            d++;
        }
        if (y <= g[x].Length - 1) d++;
        return d;
    }
}
