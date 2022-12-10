using System;
using System.IO;
using System.Linq;

namespace Day10
{
    // Puzzle description: https://adventofcode.com/2022/day/10

    class CpuInstruction
    {
        public string Opcode { get; set; } = string.Empty;
        public int Value { get; set; } = 0;
    }

    class Program
    {
        const int cycleCheckInterval = 40;
        private static List<CpuInstruction> cpuInstructions = new List<CpuInstruction>();
        private static int currentCycle = 0;
        private static int currentRowCycle = 0;
        private static int nextCycleCheck = 20;
        private static int sumOfSignalStrengths = 0;
        private static int xRegValue = 1;
        private static string displayLine = string.Empty;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 10");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            cpuInstructions = puzzleInputRaw.Select(i => new CpuInstruction() 
            {
                Opcode = i.Substring(0, 4),
                Value = i.Length > 4 ? int.Parse(i.Substring(5)) : 0
            }).ToList();

            Console.WriteLine($"* CPU instructions to process: {cpuInstructions.Count:N0}");

			PartA(cpuInstructions);

            currentCycle = 0;
            nextCycleCheck = 40;
            xRegValue = 1;
            PartB(cpuInstructions);
        }

        private static void PartA(List<CpuInstruction> cpuInstructions)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            foreach (var cpuInstruction in cpuInstructions)
            {
                currentCycle++;
                ProcessCurrentCycle();

                if (cpuInstruction.Opcode == "addx")
                {
                    currentCycle++;
                    ProcessCurrentCycle();
                    xRegValue += cpuInstruction.Value;
                }
            }

            Console.WriteLine($"*** Sum of signal strengths: {sumOfSignalStrengths:N0}");
        }

        private static void ProcessCurrentCycle()
        {
            if (currentCycle == nextCycleCheck)
            {
                Console.WriteLine($"* Cycle: {currentCycle:N0}; Signal stength: {currentCycle * xRegValue:N0}");
                sumOfSignalStrengths += currentCycle * xRegValue;
                nextCycleCheck += cycleCheckInterval;
            };
        }

        private static void PartB(List<CpuInstruction> cpuInstructions)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");


            foreach (var cpuInstruction in cpuInstructions)
            {
                currentCycle++;
                currentRowCycle++;
                displayLine += GetPixelToDraw();
                ProcessDrawCycle();

                if (cpuInstruction.Opcode == "addx")
                {
                    currentCycle++;
                    currentRowCycle++;
                    displayLine += GetPixelToDraw();
                    ProcessDrawCycle();
                    xRegValue += cpuInstruction.Value;
                }
            }
        }

        private static char GetPixelToDraw()
        {
            return (currentRowCycle - 1 >= xRegValue - 1 && currentRowCycle - 1 <= xRegValue + 1) ? '#' : '.';
        }

         private static void ProcessDrawCycle()
        {
            if (currentCycle == nextCycleCheck)
            {
                Console.WriteLine($"* Cycle: {currentCycle:D3}; {displayLine}");
                displayLine = string.Empty;
                currentRowCycle = 0;
                nextCycleCheck += cycleCheckInterval;
            };
        }
   }
}