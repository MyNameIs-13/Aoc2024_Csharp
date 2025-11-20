namespace AdventOfCode
{
    // Day_07 class which inherits from BaseDay and contains the Solve functions for the puzzle
    public class Day_07 : BaseDay
    {
        // TODO:  comment to use real puzzle data, comment out to use example puzzle data
        // protected override string InputFileDirPath => "InputsExample";

        // Define class variables
        private readonly List<Dictionary<long, long[]>> _parsed_input;

        // Constructor
        public Day_07()
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
        public static List<Dictionary<long, long[]>> ParseInput(string InputFilePath)
        {
            List<Dictionary<long, long[]>> inputList = new();
            foreach (var line in File.ReadLines(InputFilePath))
            {
                if (string.IsNullOrEmpty(line)) continue;
                var parts = line.Split(':');
                var result = long.Parse(parts[0]);
                var ints = parts[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToArray();
                inputList.Add(new Dictionary<long, long[]> { { result, ints } });
            }
            return inputList;
        }

        // Synchron implementation to receive solution for puzzle part1
        private string Solve_1_Synchron()
        {
            return calculateSolutionRecursive(_parsed_input).ToString();
        }

        // Synchron implementation to receive solution for puzzle part2
        private string Solve_2_Synchron()
        {
            return calculateSolutionRecursive(_parsed_input, true).ToString();
        }

        private long calculateSolutionRecursive(List<Dictionary<long, long[]>> inputList, bool? solve2 = false)
        {
            long solution = 0;
            foreach (var dict in inputList)
            {
                long result = dict.Keys.First();
                long[] ints = dict[result];
                if (calculationMatch(result, ints.First(), ints.Skip(1).ToArray(), solve2))
                {
                    solution += result;
                }
            }
            return solution;
        }

        private bool calculationMatch(long expectedResult, long previousResult, long[] remainingNumbers, bool? solve2 = false)
        {
            if (!remainingNumbers.Any())
            {
                return expectedResult == previousResult;
            }
            if (previousResult > expectedResult)
            {
                return false;
            }
            if (solve2 == true && calculationMatch(expectedResult, concat(previousResult, remainingNumbers.First()), remainingNumbers.Skip(1).ToArray(), solve2))
            {
                return true;
            }
            if (calculationMatch(expectedResult, previousResult * remainingNumbers.First(), remainingNumbers.Skip(1).ToArray(), solve2))
            {
                return true;
            }
            return calculationMatch(expectedResult, previousResult + remainingNumbers.First(), remainingNumbers.Skip(1).ToArray(), solve2);
        }
        private long concat(long a, long b)
        {
            return long.Parse(a.ToString() + b.ToString());
        }
    }
}
