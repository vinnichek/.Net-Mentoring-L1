using FileSystem.Logic;
using System;
using System.IO;

namespace ConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			FileSystemVisitor visitor = new FileSystemVisitor();

			visitor.OnStart += (s, e) => Console.WriteLine("Search started...");

			visitor.OnFinish += (s, e) => Console.WriteLine("Search finished.");

			visitor.OnFileFound += (s, e) => Console.WriteLine($"File {e.ItemName} found.");

			visitor.OnDirectoryFound += (s, e) =>
			{
				if (e.ItemName.Contains("qqq"))
				{
					Console.WriteLine($"Directory {e.ItemName} skipped.");
					e.Action = ActionType.Skip;
				}
				Console.WriteLine($"Directory {e.ItemName} found.");
			};

			visitor.OnFilteredFileFound += (s, e) =>
			{
				Console.WriteLine($"Filtered file {e.ItemName} found.");

				if (e.ItemName.Length == 5)
				{
					Console.WriteLine("Search stopped.");
					e.Action = ActionType.Stop;
				}
			};

			visitor.OnFilteredDirectoryFound += (s, e) => Console.WriteLine($"Filtered directory {e.ItemName} found.");

			foreach (FileSystemInfo entry in visitor.VisitDirectory("D://Test"))
			{
				Console.WriteLine(entry);
				Console.WriteLine("_________________");
			}

			Console.ReadKey();
		}
	}
}
