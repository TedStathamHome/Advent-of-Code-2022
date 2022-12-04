using System;
using System.IO;
using System.Linq;

namespace Day04
{
    // Puzzle description: https://adventofcode.com/2022/day/4

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 4");
			var assignmentPairs = File.ReadLines($"./AssignmentPairs-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA(assignmentPairs);
            PartB(assignmentPairs);
        }

        private static void PartA(List<string> assignmentPairs)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var fullyContained = CountOfFullyContained(assignmentPairs);

            Console.WriteLine($"*** Ranges fully contained in their pair: {fullyContained:N0}");
        }

        private static int CountOfFullyContained(List<string> assignmentPairs)
        {
            var fullyContained = 0;

            foreach (var assignmentPair in assignmentPairs)
            {
                var assignments = assignmentPair.Split(',');
                List<int> leftRange = assignments[0].Split('-').Select(a => int.Parse(a)).ToList();
                List<int> rightRange = assignments[1].Split('-').Select(a => int.Parse(a)).ToList();

                if (leftRange[0] >= rightRange[0] && leftRange[1] <= rightRange[1])
                {
                    fullyContained++;
                    Console.WriteLine($"* {assignmentPair}: left range is within right range");
                }
                else if (rightRange[0] >= leftRange[0] && rightRange[1] <= leftRange[1])
                {
                    fullyContained++;
                    Console.WriteLine($"* {assignmentPair}: right range is within left range");
                }
            }

            return fullyContained;
        }

        private static void PartB(List<string> assignmentPairs)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var fullyContained = CountOfFullyContained(assignmentPairs);
            var overlapped = 0;

            foreach (var assignmentPair in assignmentPairs)
            {
                var assignments = assignmentPair.Split(',');
                List<int> leftRange = assignments[0].Split('-').Select(a => int.Parse(a)).ToList();
                List<int> rightRange = assignments[1].Split('-').Select(a => int.Parse(a)).ToList();

                if ((leftRange[0] >= rightRange[0] && leftRange[1] <= rightRange[1]) || (rightRange[0] >= leftRange[0] && rightRange[1] <= leftRange[1]))
                    continue;

                if (ValueIsInRange(leftRange[0], rightRange) || ValueIsInRange(leftRange[1], rightRange)
                    || ValueIsInRange(rightRange[0], leftRange) || ValueIsInRange(rightRange[1], leftRange))
                {
                    overlapped++;
                    Console.WriteLine($"* {assignmentPair}: ranges partially overlap");
                }
            }

            Console.WriteLine($"*** # of overlapping pairs: {fullyContained + overlapped:N0}");
        }

        private static bool ValueIsInRange(int valueToCheck, List<int> range)
        {
            return valueToCheck >= range[0] && valueToCheck <= range[1]; 
        }
    }
}