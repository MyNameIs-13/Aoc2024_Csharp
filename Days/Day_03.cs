using System.Text.RegularExpressions;

namespace AdventOfCode;

// Day_03 class which inherits from BaseDay and contains the Solve functions for the puzzle
public class Day_03 : BaseDay
{
    // TODO:  comment to use real puzzle data, comment out to use example puzzle data
    // protected override string InputFileDirPath => "InputsExample";

    // Define class variables
    private readonly String _parsed_input;

    // Constructor
    public Day_03()
    {
        // Initialize class variables
        // Parse the input file here, to avoid deluting the puzzle solution times
        _parsed_input = ParseInput(InputFilePath);

        // TODO enable/disable when needed
        // LogUtils.DebugLogMode = true;
        // LogUtils.Debug($"DebugLogMode is {LogUtils.DebugLogMode}");
    }

    // Solve functions asynchron (called from Program.cs)
    public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
    public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

    // Function to parse the puzzle input into the required data structure to solve the puzzle
    public static String ParseInput(string InputFilePath)
    {
        return File.ReadAllText(InputFilePath);
    }

    // Synchron implementation to receive solution for puzzle part1
    private string Solve_1_Synchron()
    {
        var multiplicationPairs = new List<string>();
        var result = 0;
        var matches = Regex.Matches(_parsed_input, @"mul\((\d{1,3}),(\d{1,3})\)");
        foreach (Match match in matches)
        {
            int left = int.Parse(match.Groups[1].Value);
            int right = int.Parse(match.Groups[2].Value);
            result += left * right;
            multiplicationPairs.Add($"({left}, {right})");
        }
        LogUtils.Debug($"muliplication matches: {string.Join(", ", multiplicationPairs)}");
        return result.ToString();
    }

    // Synchron implementation to receive solution for puzzle part2
    private string Solve_2_Synchron()
    {
        var totalMultiplicationResult = 0;
        bool isDoActive = true; // Assumption: each start is do.

        // Regex to capture "do", "don't", or "mul(x,y)"
        // Group 1 and Group 2 are for the numbers in mul()
        var combinedPattern = @"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)";
        var allMatches = Regex.Matches(_parsed_input, combinedPattern).OrderBy(m => m.Index);

        foreach (Match match in allMatches)
        {
            if (match.Value == "do()")
            {
                isDoActive = true;
                LogUtils.Debug($"Encountered 'do()' at index {match.Index}. Do active.");
            }
            else if (match.Value == "don't()")
            {
                isDoActive = false;
                LogUtils.Debug($"Encountered 'don\'t()' at index {match.Index}. Do inactive.");
            }
            else if (match.Value.StartsWith("mul("))
            {
                if (isDoActive)
                {
                    int left = int.Parse(match.Groups[1].Value);
                    int right = int.Parse(match.Groups[2].Value);
                    totalMultiplicationResult += left * right;
                    LogUtils.Debug($"Active mul: ({left}, {right}) = {left * right} at index {match.Index}. Current total: {totalMultiplicationResult}");
                }
                else
                {
                    LogUtils.Debug($"Ignored mul: {match.Value} at index {match.Index} because 'do' is not active.");
                }
            }
        }

        return totalMultiplicationResult.ToString();
    }
}
