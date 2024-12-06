namespace AdventOfCode.Year2020;

public class Day08 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var seen = new HashSet<int>();
        int acc = 0;
        int pc = 0;

        var instructions = input.Lines().Where(IsNotBlank).ToList();

        while (!seen.Contains(pc))
        {
            seen.Add(pc);
            var parts = instructions[pc].Split(' ');
            switch (parts[0])
            {
                case "nop":
                    pc++;
                    break;
                case "acc":
                    acc += parts[1].Int();
                    pc++;
                    break;
                case "jmp":
                    pc += parts[1].Int();
                    break;
            }
        }

        return acc;
    }

    public override Answer Two(string input)
    {
        var instructions = input.Lines().Where(IsNotBlank).ToList();

        for (int swapAtPc = 0; swapAtPc < instructions.Count; swapAtPc++)
        {
            var seen = new HashSet<int>();
            int acc = 0;
            int pc = 0;

            while (!seen.Contains(pc))
            {
                seen.Add(pc);

                if (pc == instructions.Count)
                    return acc;

                var parts = instructions[pc].Split(' ');

                if (pc == swapAtPc)
                {
                    if (parts[0] == "nop") parts[0] = "jmp";
                    if (parts[0] == "jmp") parts[0] = "nop";
                }

                switch (parts[0])
                {
                    case "nop":
                        pc++;
                        break;
                    case "acc":
                        acc += parts[1].Int();
                        pc++;
                        break;
                    case "jmp":
                        pc += parts[1].Int();
                        break;
                }
            }
        }

        return 0;
    }
}
