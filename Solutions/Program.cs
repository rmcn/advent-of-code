using System.Reflection;
using AdventOfCode;

var solutions = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution))
    .Select(t => (Solution)Activator.CreateInstance(t)!)
    .Where(s => s.Year != 0)
    .OrderBy(s => s.Year)
    .ThenBy(s => s.Day)
    .ToList();


//solutions.ForEach(s => Advent.Run(s));
//Advent.Run(solutions.Single(s => s.Year == 2022 && s.Day == 6));
Advent.Run(solutions.Last());