using System;
using System.IO;
using System.Linq;

namespace Day02
{
    // Puzzle description: https://adventofcode.com/2022/day/1

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 2");
            var strategyGuide = File.ReadLines($"./StrategyGuide-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			Console.WriteLine($"* Lines in strategy guide: {strategyGuide.Count:N0}");

            PartA(strategyGuide);
            PartB(strategyGuide);
        }

        private static void PartA(List<string> strategyGuide)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var score = 0;

            foreach (var entry in strategyGuide)
            {
                score += ShapeScore(entry[2]) + OutcomeScore(entry);
            }

            Console.WriteLine($"*** Your score is: {score:N0}");
        }

        private static int ShapeScore(char shape)
        {
            return " XYZ".IndexOf(shape);
        }

        private static int OutcomeScore(string entry)
        {
            var winningOutcomes = new List<string>()
            {
                "A Y",  // Rock beaten by Paper
                "B Z",  // Paper beaten by Scissors
                "C X"   // Scissors beaten by Rock
            };

            var drawOutcomes = new List<string>()
            {
                "A X",  // Rock vs Rock
                "B Y",  // Paper vs Paper
                "C Z"   // Scissors vs Scissors
            };

            if (winningOutcomes.IndexOf(entry) != -1)
                return 6;

            if (drawOutcomes.IndexOf(entry) != -1)
                return 3;

            return 0;
        }

        private static void PartB(List<string> strategyGuide)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var score = 0;

            foreach (var entry in strategyGuide)
            {
                var decodedEntry = DecodeStrategy(entry);
                score += ShapeScore(decodedEntry[2]) + OutcomeScore(decodedEntry);
            }

            Console.WriteLine($"*** Your score is: {score:N0}");
        }

        private static string DecodeStrategy(string entry)
        {
            var result = entry.Substring(0, 2);

            if (entry[2] == 'X')        // Lose
            {
                var losingOptions = "ZXY";  // Scissors, Rock, Paper
                result += losingOptions["ABC".IndexOf(entry[0])];
                return result;
            }
            else if (entry[2] == 'Y')   // Draw
            {
                var drawingOptions = "XYZ"; // Rock, Paper, Scissors
                result += drawingOptions["ABC".IndexOf(entry[0])];
                return result;
            }
            else                        // Win
            {
                var winningOptions = "YZX"; // Paper, Scissors, Rock
                result += winningOptions["ABC".IndexOf(entry[0])];
                return result;
            }
        }
    }
}