using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class MetaHelper
{    
    public static string FilePath([CallerFilePath] string filePath = "") => filePath;

    public static int NewDay(string[] args)
    {
        Console.WriteLine($"Creating new solution file");
        
        var (year, day) = YearDay(args);

        var template = new Year0000.Day00();

        var templateSource = File.ReadAllText(template.FilePath);
        var source = templateSource.Replace("Year0000", $"Year{year}").Replace("Day00", $"Day{day}");

        var targetDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(template.FilePath))!, year);
        var targetPath = Path.Combine(targetDirectory, $"Day{day}.cs");

        Console.WriteLine($"Output path {targetPath}");

        if (File.Exists(targetPath))
            throw new Exception("Output path already exists");

        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);

        File.WriteAllText(targetPath, source);

        return 0;
    }

    private static (string year, string day) YearDay(string[] args)
    {
        var now = DateTime.Now;

        if (args.Length == 1)
        {
            if (now.Month != 12)
                throw new ArgumentException("It's not December!");
            return ($"{now.Year:0000}", $"{now.Day:00}");
        }
        if (args.Length == 3)
            return (int.Parse(args[1]).ToString("0000"), int.Parse(args[2]).ToString("00"));
        throw new ArgumentException("Invalid new args");
    }
}