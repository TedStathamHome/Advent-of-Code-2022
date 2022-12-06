using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    // Puzzle description: https://adventofcode.com/2022/day/6

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 6");
			var puzzleInput = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            Console.WriteLine($"* Datastreams to check: {puzzleInput.Count:N0}");

			PartA(puzzleInput);
            PartB(puzzleInput);
        }

        private static void PartA(List<string> dataStreams)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var currentDataStream = 1;
            const int packetSize = 4;

            foreach (var dataStream in dataStreams)
            {
                FindUniqueCharacterSequence(currentDataStream, packetSize, dataStream);
                currentDataStream++;
            }
        }

        private static void PartB(List<string> dataStreams)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var currentDataStream = 1;
            const int packetSize = 14;

            foreach (var dataStream in dataStreams)
            {
                FindUniqueCharacterSequence(currentDataStream, packetSize, dataStream);
                currentDataStream++;
            }
        }

        private static void FindUniqueCharacterSequence(int currentDataStream, int packetSize, string dataStream)
        {
            for (int i = 0; i < dataStream.Length - packetSize; i++)
            {
                var packet = dataStream.Substring(i, packetSize);
                if (packet.Distinct().Count() == packetSize)
                {
                    Console.WriteLine($"*** Datastream #{currentDataStream:N0} start-of-packet marker ends at character {i + packetSize:N0}");
                    break;
                }
            }
        }
    }
}