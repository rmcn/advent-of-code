using System.Reflection;
using AdventOfCode;

var days = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(IDay).IsAssignableFrom(t) && t.IsClass)
    .Select(t => (IDay)Activator.CreateInstance(t)!)
    .OrderBy(d => d.Year)
    .ThenBy(d => d.Day)
    .ToList();

var current = days.Last();

Advent.Run(current);