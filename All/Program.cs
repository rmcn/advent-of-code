using AdventOfCode;

var types = new[]
{
    typeof(AdventOfCode.Year2015.Day01),
    typeof(AdventOfCode.Year2016.Day01),
    typeof(AdventOfCode.Year2017.Day01),
    typeof(AdventOfCode.Year2018.Day01),
    typeof(AdventOfCode.Year2019.Day01),
    typeof(AdventOfCode.Year2020.Day01),
    typeof(AdventOfCode.Year2021.Day01),
    typeof(AdventOfCode.Year2022.Day01),
    typeof(AdventOfCode.Year2023.Day01),
    typeof(AdventOfCode.Year2024.Day01),
};

var assemblies = types.Select(t => t.Assembly).ToArray();

return Commands.Run(assemblies, args);