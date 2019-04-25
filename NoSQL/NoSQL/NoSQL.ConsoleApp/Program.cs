using NoSQL.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.ConsoleApp
{
	class Program
	{
		public static void Main(string[] args)
		{
			var library = new LibraryService();

			Console.WriteLine("Task 1.\n");
			library.AddBooks();

			Console.WriteLine("\nTask 2.\n");
			library.FindBooksWithCountMoreThanOne();

			Console.WriteLine("\nTask 3.\n");
			library.FindBooksWithMaxAndMinCount();

			Console.WriteLine("\nTask 4.\n");
			library.FindAllAuthors();

			Console.WriteLine("\nTask 5.\n");
			library.FindBooksWithoutAuthor();

			Console.WriteLine("\nTask 6.\n");
			library.IncreaseCountOfBookByOne();

			Console.WriteLine("\nTask 7.\n");
			library.AddGenreFavorityToFantasyBook();

			Console.WriteLine("\nTask 8.\n");
			library.DeleteBooksWithCountLessThanThree();

			Console.WriteLine("\nTask 9.\n");
			library.DeleteAllBooks();

			Console.ReadKey();
		}
	}
}
