namespace AdventOfCode;

// Day_05 class which inherits from BaseDay and contains the Solve functions for the puzzle
public class Day_05 : BaseDay
{
    // TODO:  comment to use real puzzle data, comment out to use example puzzle data
    // protected override string InputFileDirPath => "InputsExample";

    // Define class variables
    private readonly (Dictionary<int, HashSet<int>> pageOrder, List<int[]> printInstructions) _parsed_input;

    // Constructor
    public Day_05()
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
    public (Dictionary<int, HashSet<int>>, List<int[]>) ParseInput(string InputFilePath)
    {
        var rightAfterLeft = new Dictionary<int, HashSet<int>>();
        var printInstructions = new List<int[]>();
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (line.Contains("|"))
            {
                var leftRight = line.Split("|").Select(int.Parse).ToArray();
                if (!rightAfterLeft.TryGetValue(leftRight[1], out HashSet<int> leftSet))
                {
                    leftSet = new HashSet<int>();
                    rightAfterLeft[leftRight[1]] = leftSet;
                }
                leftSet.Add(leftRight[0]);
            }
            else
            {
                printInstructions.Add(line.Split(",").Select(int.Parse).ToArray());
            }
        }
        LogUtils.DebugDictionary(rightAfterLeft, "rightAfterLeft");
        return (rightAfterLeft, printInstructions);
    }

    // Synchron implementation to receive solution for puzzle part1
    private string Solve_1_Synchron()
    {
        int result = 0;
        foreach (int[] printInstruction in _parsed_input.printInstructions)
        {
            if (checkPrintInstruction(_parsed_input.pageOrder, printInstruction))
            {
                result += printInstruction[printInstruction.Length / 2];
            }
        }
        return result.ToString();
    }

    // Synchron implementation to receive solution for puzzle part2
    private string Solve_2_Synchron()
    {
        int result = 0;
        foreach (int[] printInstruction in _parsed_input.printInstructions)
        {
            if (checkPrintInstruction(_parsed_input.pageOrder, printInstruction)) continue;
            var orderedPrintInstruction = fixPrintInstructionOrder(_parsed_input.pageOrder, printInstruction);
            result += orderedPrintInstruction[orderedPrintInstruction.Length / 2];
        }
        return result.ToString();
    }

    private static bool checkPrintInstruction(Dictionary<int, HashSet<int>> pageOrder, int[] printInstruction)
    {
        for (var i = 0; i < printInstruction.Length; i++)
        {
            var currentPage = printInstruction[i];
            if (pageOrder.TryGetValue(currentPage, out var pagesThatMustBeBefore))
            {
                // Create a HashSet of the current and subsequent pages for efficient lookups.
                // This makes the intent of the set intersection clearer and can sometimes
                // provide a minor constant-factor performance improvement by optimizing
                // the `Contains` checks.
                var remainingPagesSet = new HashSet<int>(printInstruction.Skip(i));

                // Check if any page that must precede 'currentPage' is present
                // in the set of remainingPages.
                if (pagesThatMustBeBefore.Any(page => remainingPagesSet.Contains(page)))
                {
                    return false;
                }
            }
        }       
        return true;
    }
    
    private static int[] fixPrintInstructionOrder(Dictionary<int, HashSet<int>> pageOrder, int[] printInstruction)
    {
        List<int> orderedPrintInstruction = printInstruction.ToList();
        LogUtils.Debug(string.Join(", ", orderedPrintInstruction));

        Dictionary<int, int> pageToIndexMap = new Dictionary<int, int>();
        Action rebuildPageToIndexMap = () =>
        {
            pageToIndexMap.Clear();
            for (int k = 0; k < orderedPrintInstruction.Count; k++)
            {
                pageToIndexMap[orderedPrintInstruction[k]] = k;
            }
        };        
        
        rebuildPageToIndexMap(); // Initial build of the map

        bool correctOrder = false;
        while (!correctOrder)
        {
            correctOrder = true;
            for (var i = 0; i < orderedPrintInstruction.Count; i++) // Iterate using the list's current count
            {
                var currentPage = orderedPrintInstruction[i];
                if (pageOrder.TryGetValue(currentPage, out var pagesThatMustBeBefore))
                {
                    int maxPrerequisiteIndex = -1; // Stores the highest index of a prerequisite that appears AFTER currentPage
                    bool needsAdjustment = false;

                    foreach (var prerequisitePage in pagesThatMustBeBefore)
                    {
                        if (pageToIndexMap.TryGetValue(prerequisitePage, out int prerequisiteIndex))
                        {
                            if (prerequisiteIndex > i) // Found a prerequisite that is currently after currentPage
                            {
                                maxPrerequisiteIndex = Math.Max(maxPrerequisiteIndex, prerequisiteIndex);
                                needsAdjustment = true;
                            }
                        }
                    }

                    if (needsAdjustment)
                    {
                        // LogUtils.Debug($"Adjusting: {currentPage} at index {i}. Latest prerequisite index: {maxPrerequisiteIndex}");
                        orderedPrintInstruction.RemoveAt(i);
                        // Insert currentPage at the position of the latest prerequisite that currently appears after it.
                        // This moves currentPage before that prerequisite.
                        orderedPrintInstruction.Insert(maxPrerequisiteIndex, currentPage);
                        rebuildPageToIndexMap(); // Rebuild map after modification
                        correctOrder = false;
                        break; // Restart the outer loop as the list order has changed
                    }
                }
            }
        }     
        return orderedPrintInstruction.ToArray();
    }
}
