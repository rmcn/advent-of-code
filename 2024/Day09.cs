
namespace AdventOfCode.Year2024;

public class Day09 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        long t = 0;

        var map = input.Trim().Select(c => (int)(c - '0')).ToArray();

        var iFrom = map.Length - 1;

        var i = 0;
        var pos = 0;

        Dump(map, i, iFrom);
        while (i < map.Length - 1)
        {
            // File
            var fileId = i / 2;
            while (map[i] > 0)
            {
                t += pos * fileId;
                map[i] = map[i] - 1;
                pos++;
                Dump(map, i, iFrom);

            }
            i++;
            // Gap
            while (map[i] > 0 && iFrom > i)
            {
                fileId = iFrom / 2;
                while (map[iFrom] > 0 && map[i] > 0)
                {
                    t += pos * fileId;
                    map[i] = map[i] - 1; // decrement gap
                    map[iFrom] = map[iFrom] - 1; // decrement from
                    pos++;
                    Dump(map, i, iFrom);
                }
                if (map[iFrom] == 0)
                    iFrom -= 2;
            }
            i++;
        }

        return t;
    }

    private void Dump(int[] map, int i, int iFrom)
    {
        return;
        //LogEx(new string(' ', i) + 'v');
        //LogEx(string.Join("", map.Select(i => i.ToString())));
        //LogEx(new string(' ', iFrom) + '^');
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
