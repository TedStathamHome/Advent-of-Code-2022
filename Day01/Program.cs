using System;
using System.IO;
using System.Linq;

namespace Day01
{
    // Puzzle description: https://adventofcode.com/2022/day/1

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 1");
            //var elfCaloriesRaw = File.ReadLines(@"./ElfCalories-test.txt").ToList();
			var elfCaloriesRaw = File.ReadLines(@"./ElfCalories-full.txt").ToList();

			var elvesInParty = elfCaloriesRaw.Where(e => string.IsNullOrWhiteSpace(e)).ToList().Count + 1;

            Console.WriteLine($"* Elf calorie entries read: {elfCaloriesRaw.Count:N0}");
            Console.WriteLine($"* Elves in party: {elvesInParty:N0}");

			var elfCalories = ParseElfCalories(elfCaloriesRaw);

            PartA(elfCalories);
            PartB(elfCalories);
        }

		private static List<List<int>> ParseElfCalories(List<string> elfCaloriesRaw)
		{
			var elfCalories = new List<List<int>>();
			var calories = new List<int>();

			foreach (var elfCalorie in elfCaloriesRaw)
			{
				if (string.IsNullOrWhiteSpace(elfCalorie))
				{
					elfCalories.Add(calories);
					calories = new();
					continue;
				}

				calories.Add(int.Parse(elfCalorie));
			}

			elfCalories.Add(calories);
			Console.WriteLine($"* Number of calorie entries created: {elfCalories.Count:N0}");
			return elfCalories;
		}

        private static void PartA(List<List<int>> elfCalories)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

			var highestCalories = elfCalories.Max(e => e.Sum());

			Console.WriteLine($"*** The highest amount of calories is {highestCalories:N0}");
        }

        private static void PartB(List<List<int>> elfCalories)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

			var sortedCalories = elfCalories.Select(e => e.Sum()).OrderDescending().Take(3).ToList();

			Console.WriteLine($"* Top 3 highest calories are: {string.Join(" / ", sortedCalories)}");
			Console.WriteLine($"*** Sum of top 3 highest calories are: {sortedCalories.Sum():N0}");
        }
    }
}