namespace AdventOfCode
{
    // Day_09 class which inherits from BaseDay and contains the Solve functions for the puzzle
    public class Day_09 : BaseDay
    {
        // TODO:  comment to use real puzzle data, comment out to use example puzzle data
        // protected override string InputFileDirPath => "InputsExample";

        // Define class variables
        private readonly int[] _parsed_input;

        // Constructor
        public Day_09()
        {
            // Initialize class variables
            // Parse the input file here, to avoid deluting the puzzle solution times
            _parsed_input = File.ReadAllLines(InputFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))  // Skip empty lines
                .SelectMany(line => line.Select(c => (int)(c - '0')))  // Convert each character to ulong
                .ToArray();

            // TODO enable/disable when needed
            // LogUtils.DebugLogMode = true;
            // LogUtils.Debug($"DebugLogMode is {LogUtils.DebugLogMode}");
        }

        // Solve functions asynchron (called from Program.cs)
        public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
        public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

        // Synchron implementation to receive solution for puzzle part1
        private string Solve_1_Synchron()
        {
            var decompressed = Decompress(_parsed_input);
            var fragmented = Fragment(decompressed);
            ulong result = 0;
            for (int i = 0; i < fragmented.Count; i++)
            {
                result += (ulong)(i * fragmented[i]);
            }
            return result.ToString();
        }

        private List<int> Decompress(int[] inputData)
        {
            List<int> decompressed = new ();
            bool isFile = true;
            int i = 0;
            // First pass: build decompressed list
            foreach (int size in inputData)
            {
                int decompressedValue;
                if (isFile)
                {
                    isFile = false;
                    decompressedValue = i;
                    i++;
                }
                else
                {
                    isFile = true;
                    decompressedValue = -1;
                }

                for (int j = 0; j < size; j++)
                {
                    decompressed.Add(decompressedValue);
                }
            }
            return decompressed;
        }

        private List<int> Fragment(List<int> decompressed)
        {
            for (int i = 0; i < decompressed.Count; i++)
            {
                if (decompressed[i] == -1)
                {
                    while (true)
                    {
                        if (decompressed.Count == 0)
                            break;

                        var value = decompressed.Last();
                        decompressed.RemoveAt(decompressed.Count - 1);

                        if (value != -1)
                        {
                            decompressed[i] = value;
                            break;
                        }
                    }
                }
            }
            return decompressed;
        }

        // Synchron implementation to receive solution for puzzle part2
        private string Solve_2_Synchron()
        {
            var (decompressed, freeSpaceMap) = Decompress2(_parsed_input);
            LogUtils.DebugDictionary(freeSpaceMap, "freeSpaceMap");
            // LogUtils.Debug($"Decompressed data: {string.Join(", ", decompressed)}");
            var fragmented = Fragment2(decompressed, freeSpaceMap);
            // LogUtils.Debug($"Fragmented data: {string.Join(", ", fragmented)}");

            ulong result = 0;
            for (int i = 0; i < fragmented.Count; i++)
            {
                if (fragmented[i] != -1)
                    result += (ulong)(i * fragmented[i]);
            }
            return result.ToString();
        }

        private  (List<List<int>>, Dictionary<int, int>) Decompress2(int[] _parsed_input)
        {
            var decompressed = new List<List<int>>();
            var freeSpaceMap = new Dictionary<int, int>();
            int freeSpaceIndex = 1;
            bool isFile = true;
            int i = 0;

            foreach (int size in _parsed_input)
            {
                int decompressedValue;
                if (isFile)
                {
                    isFile = false;
                    decompressedValue = i;
                    i += 1;
                }
                else
                {
                    isFile = true;
                    decompressedValue = -1;

                    if (size > 0)
                    {
                        freeSpaceMap[freeSpaceIndex] = size;
                        freeSpaceIndex += 1;
                    }
                    freeSpaceIndex += 1;
                }
                if (size > 0)
                {
                    // Fill the segment with the current value (either file or free space)
                    List<int> currentSegment = new ();
                    for (int j = 0; j < size; j++)
                    {
                        currentSegment.Add(decompressedValue);
                    }
                    decompressed.Add(currentSegment);
                }
            }
            return (decompressed, freeSpaceMap);
        }

        private List<int> Fragment2(List<List<int>> decompressed, Dictionary<int, int> freeSpaceMap)
        {
            // Process in reverse to fill free spaces
            for (int rev_i = decompressed.Count - 1; rev_i >= 0; rev_i--)
            {
                LogUtils.Debug($"rev_i: {rev_i}");
                if (decompressed[rev_i][0] == -1) // File found
                {
                    if (freeSpaceMap.ContainsKey(rev_i)) freeSpaceMap.Remove(rev_i);
                }
                else
                {
                    // Find suitable free space
                    foreach (var entry in freeSpaceMap)
                    {
                        int index = entry.Key;
                        int size = entry.Value;

                        if (index > rev_i) break; // Skip spaces after current position

                        // Place the file in the free space
                        if (size >= decompressed[rev_i].Count)
                        {
                            int indexOffset = 0;
                            bool found = false;
                            foreach (int value in decompressed[index])
                            {
                                if (value == -1)
                                {
                                    found = true;
                                    break;
                                }
                                indexOffset++;
                            }
                            if (!found) indexOffset = 0;

                            for (int j = 0; j < decompressed[rev_i].Count; j++)
                            {
                                LogUtils.Debug($"index: {index} - j: {j} - index_offset: {indexOffset} - rev_i: {rev_i}");
                                decompressed[index][j + indexOffset] = decompressed[rev_i][j];
                                freeSpaceMap[index] -= 1;
                                decompressed[rev_i][j] = -1;
                            }

                            if (freeSpaceMap[index] == 0)
                            {
                                freeSpaceMap.Remove(index);
                            }
                            break;
                        }
                    }
                }
            }
            List<int> flattened = new List<int>();
            foreach (var sublist in decompressed)
            {
                flattened.AddRange(sublist);
            }
            return flattened;
        }

    }
}
