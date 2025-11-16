namespace AdventOfCode;


// Day_02 class which inherits from BaseDay and contains the Solve functions for the puzzle
public class Day_02 : BaseDay
{
    // TODO:  comment to use real puzzle data, comment out to use example puzzle data
    // protected override string InputFileDirPath => "InputsExample";

    // Define class variables
    private readonly List<List<int>> _parsed_input;

    // Constructor
    public Day_02()
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
    public static List<List<int>> ParseInput(string InputFilePath)
    {
        var parsed_input = new List<List<int>>();
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            parsed_input.Add(line.Split(" ").Select(int.Parse).ToList());
        }
        return parsed_input;
    }

    // Synchron implementation to receive solution for puzzle part1
    private string Solve_1_Synchron()
    {
        int safeSequenceCount = 0;
        foreach (var level in _parsed_input)
        {
            if (isLevelSafe(level)) safeSequenceCount++;
        }
        return safeSequenceCount.ToString();
    }

    // Synchron implementation to receive solution for puzzle part2
    private string Solve_2_Synchron()
    {
        int safeSequenceCount = 0;
        foreach (var level in _parsed_input)
        {
            if (isLevelSafe(level)) 
            {
                safeSequenceCount++;
            }
            else
            {
                for (int i = 0; i < level.Count; i++)
                {
                    // Create a new list that excludes the element at index i
                    var levelCopy = new List<int>(level.Count - 1);
                    for (int j = 0; j < level.Count; j++)
                    {
                        if (j != i)
                        {
                            levelCopy.Add(level[j]);
                        }
                    }
                    if (isLevelSafe(levelCopy))
                    {
                        safeSequenceCount++;
                        break;
                    }
                }
            }
        }
        return safeSequenceCount.ToString();
    }

    private bool isLevelSafe(List<int> level)
    {
        LogUtils.Debug($"Processing sequence: {string.Join(", ", level)}");

        // Rule 0: A sequence with less than 2 elements is considered safe by default
        if (level.Count < 2)
        {
            LogUtils.Debug($"Sequence: {string.Join(", ", level)} IS SAFE (less than 2 elements).\n");
            return true;
        }

        bool isIncreasing = (level[1] - level[0] > 0);
        int previousValue = level[0];

        for (int i = 1; i < level.Count; i++)
        {
            int currentValue = level[i];
            int diff = currentValue - previousValue;
            int absDiff = Math.Abs(diff);

            // Rule 1: absolute difference between numbers must be between 1 and 3 (inclusive)
            if (absDiff < 1 || absDiff > 3)
            {
                LogUtils.Debug($"Violation: absDiff between {previousValue} and {currentValue} is {absDiff}, not within [1, 3]. Sequence is NOT SAFE.\n");
                return false;
            }
            // Rule 2: numbers in sequence are either only increasing or only decreasing
            if ((isIncreasing && diff < 0) || (!isIncreasing && diff > 0))
            {
                LogUtils.Debug($"Violation: Expected {(isIncreasing ? "increasing" : "decreasing")}, but found {previousValue} -> {currentValue}. Sequence is NOT SAFE.\n");
                return false;
            }
            previousValue = currentValue; // Update previousValue for the next iteration
        }

        LogUtils.Debug($"Sequence: {string.Join(", ", level)} IS SAFE.\n");
        return true;
    }
}
