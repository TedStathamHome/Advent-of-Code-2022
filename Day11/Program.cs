using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    // Puzzle description: https://adventofcode.com/2022/day/11

    class Monkey
    {
        public List<long> HeldItems { get; set; } = new();
        public int InspectedItems { get; set; } = 0;
        public Char WorryOp { get; set; } = ' ';
        public bool WorryOpValueIsOldValue { get; set; } = false;
        public int WorryOpValue { get; set; } = 0;
        public int DivTestValue { get; set; } = 1;
        public int ThrowToMonkeyOnTrue { get; set; } = 0;
        public int ThrowToMonkeyOnFalse { get; set; } = 0;
    }

    class Program
    {
        private static List<Monkey> monkeys = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 11");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            ParsePuzzleDataIntoMonkeys(puzzleInputRaw);
            Console.WriteLine($"* Loaded {monkeys.Count:N0} monkeys");

			PartA();

            ParsePuzzleDataIntoMonkeys(puzzleInputRaw);
            PartB();
        }

        private static void ParsePuzzleDataIntoMonkeys(List<string> puzzleInput)
        {
            monkeys.Clear();

            for (int i = 0; i < puzzleInput.Count; i += 7)
            {
                var monkey = new Monkey();
                
                monkey.HeldItems = puzzleInput[i + 1].Replace("  Starting items: ", "")
                    .Split(',').ToList().Select(h => long.Parse(h)).ToList();

                var opDetails = puzzleInput[i + 2].Trim().Split(' ').ToList();
                monkey.WorryOp = opDetails[4][0];
                monkey.WorryOpValueIsOldValue = opDetails[5] == "old";
                monkey.WorryOpValue = opDetails[5] == "old" ? (int)0 : int.Parse(opDetails[5]);

                monkey.DivTestValue = int.Parse(puzzleInput[i + 3].Trim().Split(' ').ToList()[3]);
                monkey.ThrowToMonkeyOnTrue = int.Parse(puzzleInput[i + 4].Trim().Split(' ').ToList()[5]);
                monkey.ThrowToMonkeyOnFalse = int.Parse(puzzleInput[i + 5].Trim().Split(' ').ToList()[5]);

                monkeys.Add(monkey);
            }
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            for (int r = 0; r < 20; r++)
            {
                for (int m = 0; m < monkeys.Count; m++)
                {
                    MonkeyBusiness(m);
                }
            }

            var busiestMonkeys = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).ToList();
            var monkeyBusiness = busiestMonkeys[0].InspectedItems * busiestMonkeys[1].InspectedItems;
            Console.WriteLine($"*** Monkey business level: {monkeyBusiness:N0}");
        }

        private static void MonkeyBusiness(int monkey, bool adjustWorryLevel = true, long worryReductionFactor = 1)
        {
            foreach (int item in monkeys[monkey].HeldItems)
            {
                long newItem = item;
                long worryOpValue = monkeys[monkey].WorryOpValueIsOldValue ? newItem : monkeys[monkey].WorryOpValue;

                if (monkeys[monkey].WorryOp == '*')
                {
                    newItem = newItem * worryOpValue;
                }
                else
                {
                    newItem = newItem + worryOpValue;
                }

                if (adjustWorryLevel == true)
                {
                    newItem = (long)Math.Floor(newItem / 3.0);
                }
                else
                {
                    newItem = newItem % worryReductionFactor;
                }
                
                monkeys[monkey].InspectedItems++;

                int monkeyToThrowTo = -1;
                if (newItem % monkeys[monkey].DivTestValue == (int)0)
                {
                    monkeyToThrowTo = monkeys[monkey].ThrowToMonkeyOnTrue;
                }
                else
                {
                    monkeyToThrowTo = monkeys[monkey].ThrowToMonkeyOnFalse;
                }

                monkeys[monkeyToThrowTo].HeldItems.Add(newItem);
            }

            monkeys[monkey].HeldItems.Clear();
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            for (int m = 0; m < monkeys.Count; m++)
            {
                Console.WriteLine($"** Monkey {m}: {monkeys[m].InspectedItems:N0}");
            }

            // this works because all the DivTestValues are prime numbers
            long worryReductionFactor = monkeys
                .Select(m => m.DivTestValue)
                .Aggregate((f1, f2) => f1 * f2);

            Console.WriteLine($"* Worry reduction factor: {worryReductionFactor:N0}");

            const bool dontAdjustWorryLevel = false;
            for (int r = 0; r < 10_000; r++)
            {
                for (int m = 0; m < monkeys.Count; m++)
                {
                    MonkeyBusiness(m, dontAdjustWorryLevel, worryReductionFactor);
                }

                if (r == 999)
                {
                    Console.WriteLine($"* --- Round {r + 1}");
                    for (int m = 0; m < monkeys.Count; m++)
                    {
                        Console.WriteLine($"** Monkey {m}: {monkeys[m].InspectedItems:N0}");
                    }
                }
            }

            Console.WriteLine("* ----------");
            for (int m = 0; m < monkeys.Count; m++)
            {
                Console.WriteLine($"** Monkey {m}: {monkeys[m].InspectedItems:N0}");
            }

            var busiestMonkeys = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).ToList();
            long monkeyBusiness = (long)busiestMonkeys[0].InspectedItems * (long)busiestMonkeys[1].InspectedItems;
            Console.WriteLine($"*** Monkey business level: {monkeyBusiness:N0}");
       }
    }
}