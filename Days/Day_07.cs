namespace AdventOfCode
{
    // Day_07 class which inherits from BaseDay and contains the Solve functions for the puzzle
    public class Day_07 : BaseDay
    {
        // TODO:  comment to use real puzzle data, comment out to use example puzzle data
        // protected override string InputFileDirPath => "InputsExample";

        // Define class variables
        private readonly List<PuzzleEntry> _parsedInput;

        // Constructor
        public Day_07()
        {
            // Initialize class variables
            // Parse the input file here, to avoid diluting the puzzle solution times
            _parsedInput = ParseInput(InputFilePath);

            // TODO enable/disable when needed
            // LogUtils.DebugLogMode = true;
            // LogUtils.Debug($"DebugLogMode is {LogUtils.DebugLogMode}");
        }

        // Solve functions asynchron (called from Program.cs)
        public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
        public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

        // Struct to hold a puzzle entry
        public struct PuzzleEntry
        {
            public ulong Result { get; init; }
            public ulong[] Numbers { get; init; }
        }

        // Function to parse the puzzle input into the required data structure to solve the puzzle
        public static List<PuzzleEntry> ParseInput(string inputFilePath)
        {
            var list = new List<PuzzleEntry>();
            foreach (var line in File.ReadLines(inputFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(':');
                var result = ulong.Parse(parts[0]);
                var numbers = parts[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(ulong.Parse)
                    .ToArray();
                list.Add(new PuzzleEntry { Result = result, Numbers = numbers });
            }
            return list;
        }

        // Synchron implementation to receive solution for puzzle part1
        private string Solve_1_Synchron() => CalculateSolution(_parsedInput).ToString();

        // Synchron implementation to receive solution for puzzle part2
        private string Solve_2_Synchron() => CalculateSolution(_parsedInput, true).ToString();

        private ulong CalculateSolution(List<PuzzleEntry> entries, bool solve2 = false)
        {
            ulong total = 0;
            foreach (var e in entries)
            {
                if (Matches(e.Result, e.Numbers[0], e.Numbers[1..], solve2))
                    total += e.Result;
            }
            return total;
        }

        private static bool Matches(ulong expected, ulong current, Span<ulong> rest, bool solve2)
        {
            if (rest.Length == 0)
                return expected == current;

            if (current > expected)
                return false;

            var next = rest[0];
            var tail = rest[1..];

            if (solve2 && Matches(expected, Concat(current, next), tail, solve2))
                return true;

            if (Matches(expected, current * next, tail, solve2))
                return true;

            return Matches(expected, current + next, tail, solve2);
        }

        private static ulong Concat(ulong a, ulong b)
        {
            var factor = (ulong)Math.Pow(10, Math.Floor(Math.Log10(b) + 1));
            return a * factor + b;
        }
    }
}