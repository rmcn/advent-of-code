using System.Reflection;

namespace AdventOfCode;

public static class Commands
{
    public static int Run(string[] args) => Run(new Assembly[] { Assembly.GetCallingAssembly() }, args);

    public static int Run(Assembly[] assemblies, string[] args)
    {
        switch (args.FirstOrDefault())
        {
            case "new":
                // Create a new solution file via:
                //    dotnet run -- new [year day]
                return MetaHelper.NewDay(args);

            default:
                foreach (var solution in Solutions(assemblies, args))
                {
                    Advent.Run(solution);
                }
                return 0;
        }
    }

    private static IEnumerable<Solution> Solutions(Assembly[] assemblies, IEnumerable<string> args)
    {
        var solutions = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution))
            .Select(t => (Solution)Activator.CreateInstance(t)!);

        var filter = args.ToList();

        if (filter.Count == 0)
            return new [] { solutions.OrderBy(s => s.Year).ThenBy(s => s.Day).Last() };

        if (filter.FirstOrDefault() == "all")
            return solutions;

        if (assemblies.Length == 1)
        {
            if (filter.Count() != 1 || !int.TryParse(filter[0], out var dayOnly))
                throw new ArgumentException("Expected day");

            return new [] { solutions.OrderBy(s => s.Year).Where(s => s.Day == dayOnly).Last() };
        }

        if (filter.Count() != 2 || !int.TryParse(filter[0], out var year) || !int.TryParse(filter[1], out var day))
            throw new ArgumentException("Expected year day");
            
        return new [] { solutions.Where(s => s.Year == year && s.Day == day).Last() };
    }
}
