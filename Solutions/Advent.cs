using System.Collections.Specialized;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using AdventOfCode;

static class Advent
{
    private static readonly HttpClient Client = new(new HttpClientHandler { UseCookies = false }) { BaseAddress = new Uri("https://adventofcode.com/") };

    private static string CacheDir => Environment.GetEnvironmentVariable("AOC_CACHE_DIR") ?? "";

    private static string PathFor(IDay day, string type)
    {
        var dir = Path.Combine(CacheDir, $"{day.Year}", $"{day.Day:00}");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"{day.Day:00}-{type}.txt");
    }

    private static string Cookie() => File.ReadAllText(Path.Combine(CacheDir, "Cookie.txt"));

    static string InputFor(IDay day)
    {
        var path = PathFor(day, "Input");
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{day.Year}/day/{day.Day}/input");
        request.Headers.Add("Cookie", Cookie());

        var result = Client.Send(request);
        result.EnsureSuccessStatusCode();

        var input = result.Content.ReadAsStringAsync().Result;

        File.WriteAllText(path, input);
        return input;
    }

    static string QuestionFor(IDay day)
    {
        var path = PathFor(day, "Question");

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{day.Year}/day/{day.Day}");
        request.Headers.Add("Cookie", Cookie());

        var result = Client.Send(request);
        result.EnsureSuccessStatusCode();

        var question = result.Content.ReadAsStringAsync().Result;

        File.WriteAllText(path, question);

        return question;
    }

    static string Example(string question)
    {
        var candidate = question.Lines()
            .SkipWhile(l => !(l.StartsWith("<p>") && l.ToLower().Contains("for example")))
            .Skip(1)
            .TakeWhile(l => l != "</code></pre>")
            .ToList();

        var example = "";

        if ((candidate.FirstOrDefault() ?? "").StartsWith("<pre><code>"))
        {
            example = string.Join('\n', candidate).Substring(11);

            Console.WriteLine($"Found example:\n{example}");
        }
        else
        {
            Console.WriteLine("No example found");
        }

        return example;
    }

    static void Submit(IDay day, int level, (bool submit, string value) answer)
    {
        var path = PathFor(day, $"{level}-Solution");
        var submitted = File.Exists(path) && File.ReadAllText(path) == answer.value;

        var action = !answer.submit ? "View" : submitted ? "Submitted" : "Submitting";
        Console.WriteLine($"{action} {day.Year} {day.Day} {level}: {answer.value}");

        if (!answer.submit || submitted)
        {
            return;
        }

        File.WriteAllText(path, answer.value);

        var nvc = new Dictionary<string, string>()
        {
            { "level", level.ToString() },
            { "answer", answer.value }
        };


        var request = new HttpRequestMessage(HttpMethod.Post, $"{day.Year}/day/{day.Day}/answer")
        {
            Content = new FormUrlEncodedContent(nvc)
        };
        request.Headers.Add("Cookie", Cookie());

        var result = Client.Send(request);
        result.EnsureSuccessStatusCode();

        var text = result.Content.ReadAsStringAsync().Result;

        var article = string.Join('\n', text.Split("\n").Where(l => l.Contains("<article>")));

        Console.WriteLine(article);
    }

    public static void Run(IDay day)
    {
        Console.WriteLine($"Running {day.Year} {day.Day}");

        var question = QuestionFor(day);

        var example = Example(question);
        if (example != "")
        {
            var exampleAnswer = day.One(example);
            Console.WriteLine($"Answer: {exampleAnswer}");
        }

        var input = InputFor(day);

        Submit(day, 1, day.One(input));
        Submit(day, 2, day.Two(input));
    }
}