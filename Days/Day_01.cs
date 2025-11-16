namespace AdventOfCode;


// Day_XY class which inherits from BaseDay and contains the Solve functions for the puzzle
public class Day_01 : BaseDay
{
    // TODO:  comment to use real puzzle data, comment out to use example puzzle data
    protected override string InputFileDirPath => "InputsExample";

    // Define class variables
    private readonly (List<int>, List<int>, Dictionary<int, int>) _input;

    // Constructor
    public Day_01()
    {
        // Initialize class variables
        // Parse the input file here, to avoid deluting the puzzle solution times
        _input = ParseFileToList(InputFilePath);
    }

    // Solve functions asynchron (called from Program.cs)
    public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
    public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

    // Synchron implementation to receive solution for puzzle part1
    private string Solve_1_Synchron()
    {
        var result = 0;
        var leftValues = _input.Item1;
        leftValues.Sort();
        var rightValues = _input.Item2;
        rightValues.Sort();
        for (int i = 0; i < leftValues.Count; i++)    
        {
            result += Math.Abs(rightValues[i] - leftValues[i]);
        }
        return result.ToString();
    }
    
    // Synchron implementation to receive solution for puzzle part1
    private string Solve_2_Synchron()
    {
        var result = 0;
        var leftValues = _input.Item1;
        Dictionary<int, int> rightOccurences = _input.Item3;
        foreach (var value in leftValues)
        {
            result += (value * rightOccurences.GetValueOrDefault(value, 0));
        }
        return result.ToString();
    }

    // Function to parse the puzzle input into the required data structure to solve the puzzle
    public static (List<int>, List<int>, Dictionary<int, int>) ParseFileToList(string inputFilePath)
    {
        var rightOccurences = new Dictionary<int, int>();
        var leftValues = new List<int>();
        var rightValues = new List<int>();
        foreach (var line in File.ReadLines(inputFilePath))
        {
            if (line == "") continue;
            var parts = line.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;

            leftValues.Add(int.Parse(parts[0]));
            var rightValue = int.Parse(parts[1]);
            rightValues.Add(rightValue);
            rightOccurences[rightValue] = rightOccurences.GetValueOrDefault(rightValue, 0) + 1;
        }
        return (leftValues, rightValues, rightOccurences);
    }
}
