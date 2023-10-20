using System.Collections.Specialized;
using System.Net;
using System.Text;
using AdventOfCode;

static class Advent
{
    private static string CacheDir => Environment.GetEnvironmentVariable("AOC_CACHE_DIR") ?? "";

    private static string Cookie() => File.ReadAllText(Path.Combine(CacheDir, "Cookie.txt"));

    static string InputFor(IDay day)
    {
        var path = Path.Combine(CacheDir, day.Year.ToString(), $"{day.Day:00}-Input.txt");
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        using var wc = new WebClient();
        wc.Headers.Add(HttpRequestHeader.Cookie, Cookie());
        var input = wc.DownloadString($"https://adventofcode.com/{day.Year}/day/{day.Day}/input");
        File.WriteAllText(path, input);
        return input;
    }

    static void Submit(IDay day, int level, (bool submit, string value) answer)
    {
        var path = Path.Combine(CacheDir, day.Year.ToString(), $"{day.Day:00}-{level}-Solution.txt");
        var submitted = File.Exists(path) && File.ReadAllText(path) == answer.value;

        var action = !answer.submit ? "View" : submitted ? "Submitted" : "Submitting";
        Console.WriteLine($"{action} {day.Year} {day.Day} {level}: {answer.value}");

        if (!answer.submit || submitted)
        {
            return;
        }

        File.WriteAllText(path, answer.value);

        var nvc = new NameValueCollection()
        {
            { "level", level.ToString() },
            { "answer", answer.value }
        };

        using var wc = new WebClient();
        wc.Headers.Add(HttpRequestHeader.Cookie, Cookie());
        var result = wc.UploadValues($"https://adventofcode.com/{day.Year}/day/{day.Day}/answer", "POST", nvc);
        var text = Encoding.UTF8.GetString(result);

        var article = string.Join('\n', text.Split("\n").Where(l => l.Contains("<article>")));

        Console.WriteLine(article);
    }

    public static void Run(IDay day)
    {
        Console.WriteLine($"Running {day.Year} {day.Day}");

        var input = InputFor(day);

        Submit(day, 1, day.One(input));
        Submit(day, 2, day.Two(input));
    }
}