using System.Collections.Specialized;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using AdventOfCode;

static class Advent
{
    private static readonly HttpClient Client = new(new HttpClientHandler { UseCookies = false }) { BaseAddress = new Uri("https://adventofcode.com/") };

    private static string CacheDir => Environment.GetEnvironmentVariable("AOC_CACHE_DIR") ?? "";

    private static string PathFor(Solution solution, string type)
    {
        var dir = Path.Combine(CacheDir, $"{solution.Year}", $"{solution.Day:00}");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"{solution.Day:00}-{type}.txt");
    }

    private static string Cookie() => File.ReadAllText(Path.Combine(CacheDir, "Cookie.txt"));

    static string InputFor(Solution solution)
    {
        var path = PathFor(solution, "Input");
        Console.WriteLine($"Reading input {path}");
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{solution.Year}/day/{solution.Day}/input");
        request.Headers.Add("Cookie", Cookie());

        var result = Client.Send(request);
        result.EnsureSuccessStatusCode();

        var input = result.Content.ReadAsStringAsync().Result;

        File.WriteAllText(path, input);
        return input;
    }

    static string QuestionFor(Solution solution)
    {
        var path = PathFor(solution, "Question");

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{solution.Year}/day/{solution.Day}");
        request.Headers.Add("Cookie", Cookie());

        var result = Client.Send(request);
        result.EnsureSuccessStatusCode();

        var question = result.Content.ReadAsStringAsync().Result;

        File.WriteAllText(path, question);

        return question;
    }

    static string Example(Solution solution, string question)
    {
        if (solution.Example != string.Empty)
        {
            Console.WriteLine($"Solution example:\n{solution.Example}");
            return solution.Example;
        }

        var candidate = question.Lines()
            .SkipWhile(l => !(l.StartsWith("<p>") && l.ToLower().Contains("example")))
            .Skip(1)
            .TakeUntil(l => l.EndsWith("</code></pre>"))
            .ToList();

        var example = "";

        if ((candidate.FirstOrDefault() ?? "").StartsWith("<pre><code>"))
        {
            int iLast = candidate.Count - 1;
            if (candidate[iLast].EndsWith("</code></pre>"))
                candidate[iLast] = candidate[iLast][..^13];

            if (candidate[iLast] == string.Empty)
                candidate.RemoveAt(iLast);

            example = string.Join('\n', candidate)[11..];

            Console.WriteLine($"Found example:\n{example}");
        }
        else
        {
            Console.WriteLine("No example found");
        }

        return example;
    }

    static void Submit(Solution solution, int level, object answer)
    {
        var path = PathFor(solution, $"{level}-Solution");
        var submitted = File.Exists(path) && File.ReadAllText(path) == answer.ToString();

        var submit = answer is SubmitAnswer;
        var action = !submit ? "View" : submitted ? "Submitted" : "Submitting";
        Console.WriteLine($"{action} {solution.Year} {solution.Day} {level}: {answer}");

        if (!submit || submitted)
        {
            return;
        }

        File.WriteAllText(path, answer.ToString());

        var nvc = new Dictionary<string, string>()
        {
            { "level", level.ToString() },
            { "answer", answer.ToString() ?? string.Empty }
        };


        var request = new HttpRequestMessage(HttpMethod.Post, $"{solution.Year}/day/{solution.Day}/answer")
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

    public static void Run(Solution solution)
    {
        Console.WriteLine($"Running {solution.Year} {solution.Day}");

        var question = QuestionFor(solution);

        var example = Example(solution, question);
        if (example != "")
        {
            solution.LogEx = Console.WriteLine;
            Console.WriteLine($"Example 1: {solution.One(example)}");
            Console.WriteLine($"Example 2: {solution.Two(example)}");
            solution.LogEx = (string _) => { };
        }

        var input = InputFor(solution);

        Submit(solution, 1, solution.One(input));
        Submit(solution, 2, solution.Two(input));
    }
}