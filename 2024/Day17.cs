namespace AdventOfCode.Year2024;

public class Day17 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var lines = input.Lines();
        var a = lines[0].Long();
        var b = lines[1].Long();
        var c = lines[2].Long();

        var mem = lines[4].Longs();

        int ip = 0;

        var output = new List<long>();

        while (ip >= 0 && ip < mem.Count)
        {
            var co = Combo(a, b, c, mem[ip + 1]);
            switch (mem[ip])
            {
                case 0: // adv
                    a /= (int)Pow(2, co);
                    ip += 2;
                    break;
                case 1: // bxl
                    b ^= mem[ip + 1];
                    ip += 2;
                    break;
                case 2: // bst
                    b = co % 8;
                    ip += 2;
                    break;
                case 3: // jnz
                    ip = a == 0 ? ip + 2 : (int)mem[ip + 1];
                    break;
                case 4: // bxc
                    b ^= c;
                    ip += 2;
                    break;
                case 5: // out
                    output.Add(co % 8);
                    ip += 2;
                    break;
                case 6: // bdv
                    b = a / (int)Pow(2, co);
                    ip += 2;
                    break;
                case 7:
                    c = a / (int)Pow(2, co);
                    ip += 2;
                    break;
                default:
                    throw new Exception();
            }
        }

        return string.Join(",", output);
    }

    private long Combo(long a, long b, long c, long v)
    {
        return v switch 
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => a,
            5 => b,
            6 => c,
            _ => throw new Exception()
        };
    }


    // Log a values that get to length 7 and look at diffs to get start and increment
    // Repeat with longer value
    public override Answer Two(string input)
    {
        var lines = input.Lines();
        long a = 0;
        var b = lines[1].Long();
        var c = lines[2].Long();

        var mem = lines[4].Longs();

        while (true)
        {
            if (Emulate(a, b, c, mem))
                return a;
            a += 1;

            //if (a % 100000 == 0)
            //    Console.Write(".");
        }
    }

    private bool Emulate(long a, long b, long c, List<long> mem)
    {
        var initA = a;
        var ip = 0;
        var outIndex = 0;

        while (ip >= 0 && ip < mem.Count)
        {
            var co = Combo(a, b, c, mem[ip + 1]);
            switch (mem[ip])
            {
                case 0: // adv
                    a /= (int)Pow(2, co);
                    ip += 2;
                    break;
                case 1: // bxl
                    b ^= mem[ip + 1];
                    ip += 2;
                    break;
                case 2: // bst
                    b = co % 8;
                    ip += 2;
                    break;
                case 3: // jnz
                    ip = a == 0 ? ip + 2 : (int)mem[ip + 1];
                    break;
                case 4: // bxc
                    b ^= c;
                    ip += 2;
                    break;
                case 5: // out
                    if (outIndex > mem.Count || mem[outIndex] != (co % 8))
                        return false;
                    if (outIndex == 7)
                        Console.WriteLine($"{initA}");
                    outIndex++;
                    ip += 2;
                    break;
                case 6: // bdv
                    b = a / (int)Pow(2, co);
                    ip += 2;
                    break;
                case 7:
                    c = a / (int)Pow(2, co);
                    ip += 2;
                    break;
                default:
                    throw new Exception();
            }
        }
        return outIndex == mem.Count;
    }
}
