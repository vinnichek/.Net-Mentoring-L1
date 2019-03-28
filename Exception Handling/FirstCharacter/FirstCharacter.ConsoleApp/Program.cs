using System;

namespace FirstCharacter.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			string inputString = string.Empty;

			Console.WriteLine("Please, enter strings.. (Press ESC to exit.)");

			while (Console.ReadKey(true).Key != ConsoleKey.Escape)
			{
				inputString = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(inputString))
				{
					Console.WriteLine("You've entered empty string! Try again.");
					continue;
				}

				Console.WriteLine($"First character is '{inputString[0]}'.");
			}
		}
	}
}
