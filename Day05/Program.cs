using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    // Puzzle description: https://adventofcode.com/2022/day/5

    class Program
    {
        private static List<Stack<char>> CrateStacks = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 5");
			List<string> puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
            var stackListLine = BuildCrateStacks(puzzleInputRaw);
			PartA(puzzleInputRaw, stackListLine);

            stackListLine = BuildCrateStacks(puzzleInputRaw);
            PartB(puzzleInputRaw, stackListLine);
        }

        private static int BuildCrateStacks(List<string> puzzleInputRaw)
        {
            var stackListLine = -1;
            var numberOfStacks = 0;
            CrateStacks = new();

            for (int i = 0; i < puzzleInputRaw.Count; i++)
            {
                if (puzzleInputRaw[i].Substring(0, 3) == " 1 ")
                {
                    stackListLine = i;
                    numberOfStacks = puzzleInputRaw[i].Replace(" ", "").Length;
                    Console.WriteLine($"* There are {numberOfStacks} stacks");

                    for (int j = 0; j < numberOfStacks; j++)
                    {
                        CrateStacks.Add(new Stack<char>());
                    }

                    break;
                }
            }

            for (int i = stackListLine - 1; i > -1; i--)
            {
                for (int j = 0; j < numberOfStacks; j++)
                {
                    var crateCode = puzzleInputRaw[i][j * 4 + 1];

                    if (crateCode != ' ')
                        CrateStacks[j].Push(crateCode);
                }
            }

            return stackListLine;
        }

        private static void PartA(List<string> puzzleInput, int stackListLine)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            for (int i = stackListLine + 2; i < puzzleInput.Count; i++)
            {
                var instruction = puzzleInput[i].Split(' ');
                var howManyToMove = int.Parse(instruction[1]);
                var stackToMoveFrom = int.Parse(instruction[3]) - 1;
                var stackToMoveTo = int.Parse(instruction[5]) - 1;

                for (int j = 0; j < howManyToMove; j++)
                {
                    var crateCode = CrateStacks[stackToMoveFrom].Pop();
                    CrateStacks[stackToMoveTo].Push(crateCode);                    
                }
            }

            var topCrates = string.Empty;
            for (int i = 0; i < CrateStacks.Count; i++)
            {
                if (CrateStacks[i].TryPeek(out char crateCode))
                {
                    topCrates += crateCode;
                }
            }

            Console.WriteLine($"*** Top crates are: {topCrates}");
        }

        private static void PartB(List<string> puzzleInput, int stackListLine)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
 
             for (int i = stackListLine + 2; i < puzzleInput.Count; i++)
            {
                var instruction = puzzleInput[i].Split(' ');
                var howManyToMove = int.Parse(instruction[1]);
                var stackToMoveFrom = int.Parse(instruction[3]) - 1;
                var stackToMoveTo = int.Parse(instruction[5]) - 1;

                var cratesToMoveAtOnce = "";

                for (int j = 0; j < howManyToMove; j++)
                {
                    var crateCode = CrateStacks[stackToMoveFrom].Pop();
                    cratesToMoveAtOnce = crateCode + cratesToMoveAtOnce;
                }

                foreach (var crateCode in cratesToMoveAtOnce)
                {
                    CrateStacks[stackToMoveTo].Push(crateCode);
                }
            }

            var topCrates = string.Empty;
            for (int i = 0; i < CrateStacks.Count; i++)
            {
                if (CrateStacks[i].TryPeek(out char crateCode))
                {
                    topCrates += crateCode;
                }
            }

            Console.WriteLine($"*** Top crates are: {topCrates}");
       }
    }
}