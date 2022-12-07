using System;
using System.IO;
using System.Linq;

namespace Day07
{
    // Puzzle description: https://adventofcode.com/2022/day/7

	class FileInfo
	{
		public string Path { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public long Size { get; set; } = 0;
	}

    class Program
    {
		private static List<string> currentPath = new();
		private static List<string> paths = new();
		private static List<FileInfo> files = new();

		static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 7");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			BuildFileSystem(puzzleInputRaw);

			PartA();
            PartB();
        }

		private static void BuildFileSystem(List<string> puzzleInput)
		{
			paths.Clear();
			files.Clear();
			SetCurrentPathToRoot();
			AddCurrentPathToList();

			var commandLine = 0;

			while (commandLine < puzzleInput.Count)
			{
				var command = puzzleInput[commandLine];
				if (command[..4] == "$ cd")
				{
					if (command == "$ cd /")
					{
						SetCurrentPathToRoot();
					}
					else
					{
						var pathToSwitchTo = command[5..].Trim();
						
						if (pathToSwitchTo == "..")
						{
							currentPath.RemoveAt(currentPath.Count - 1);
						}
						else
						{
							currentPath.Add(pathToSwitchTo);
							AddCurrentPathToList();
						}
					}
				}
				else if (command[..4] == "$ ls")
				{
					commandLine++;

					var filePath = CurrentPath();

					if (files.Count(f => f.Path == filePath) > 0)
						files.RemoveAll(f => f.Path == filePath);

					while (commandLine < puzzleInput.Count)
					{
						var fileDetails = puzzleInput[commandLine].Split(' ');

						if (fileDetails[0] == "dir")
						{
							currentPath.Add(fileDetails[1]);
							AddCurrentPathToList();
							currentPath.RemoveAt(currentPath.Count - 1);
						}
						else
						{
							files.Add(new FileInfo()
							{
								Path = filePath,
								Name = fileDetails[1],
								Size = long.Parse(fileDetails[0])
							});
						}

						if (commandLine + 1 < puzzleInput.Count && puzzleInput[commandLine + 1][0] == '$')
							break;

						commandLine++;
					}
				}

				commandLine++;
			}

			Console.WriteLine("* == Paths discovered");
			foreach (var path in paths.Order().ToList())
			{
				Console.WriteLine($"* {path}");
			}

			Console.WriteLine("\r\n* == File details discovered");
			var lastPath = string.Empty;
			foreach (var file in files.OrderBy(f => f.Path).ThenBy(f => f.Name).ToList())
			{
				if (lastPath != file.Path)
				{
					Console.WriteLine($"* In path > {file.Path}");
					lastPath= file.Path;
				}

				Console.WriteLine($"* > {file.Name} ({file.Size:N0} bytes)");
			}
		}

		private static string CurrentPath()
		{
			// ensures the path starts and ends with a slash ( / )
			var path = string.Join('/', currentPath).Replace("//", "/");
			path += path.Length > 1 ? "/" : "";

			return path;
		}

		private static void AddCurrentPathToList()
		{
			var pathToAdd = CurrentPath();

			if (!paths.Contains(pathToAdd))
				paths.Add(pathToAdd);
		}

		private static void SetCurrentPathToRoot()
		{
			currentPath.Clear();
			currentPath.Add("/");
		}

		private static Dictionary<string, long> CalculateFolderSizes()
		{
			var folderSizes = new Dictionary<string, long>();

			foreach (var path in paths.Order().ToList())
			{
				var folderSize = files.Where(f => f.Path.StartsWith(path)).Sum(f => f.Size);
				folderSizes.Add(path, folderSize);

				//Console.WriteLine($"* {path} contains {folderSize:N0} bytes of files");
			}

			return folderSizes;
		}

		private static void PartA()
		{
			Console.WriteLine("\r\n**********");
			Console.WriteLine("* Part A");

			var folderSizes = CalculateFolderSizes();

			var sumOfFoldersContaining100_000BytesOrFewer = folderSizes.Where(s => s.Value <= 100000).Sum(s => s.Value);
			Console.WriteLine($"*** Sum of folders containing 100,000 bytes or fewer: {sumOfFoldersContaining100_000BytesOrFewer:N0}");
		}

		private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

			const long totalDiskSpace = 70_000_000;
			const long minimumFreeDiskSpace = 30_000_000;

			var folderSizes = CalculateFolderSizes();
			var usedDiskSpace = folderSizes["/"];
			Console.WriteLine($"* Used disk space: {usedDiskSpace:N0} / {totalDiskSpace:N0}");

			var diskSpaceToFreeUp = minimumFreeDiskSpace - (totalDiskSpace - usedDiskSpace);
			Console.WriteLine($"* Disk space to free up: {diskSpaceToFreeUp:N0}");

			var potentialPathsToDelete = folderSizes.Where(s => s.Value >= diskSpaceToFreeUp).OrderBy(s => s.Value).Take(1).ToList();
			Console.WriteLine($"*** Folder to delete: {potentialPathsToDelete[0].Key} of size {potentialPathsToDelete[0].Value:N0}");
		}
    }
}