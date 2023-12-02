using System.Reflection;
using AdventOfCode;

var days = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution))
    .Select(t => (Solution)Activator.CreateInstance(t)!)
    .OrderBy(d => d.Year)
    .ThenBy(d => d.Day)
    .ToList();

var current = days.Last();

Advent.Run(current);