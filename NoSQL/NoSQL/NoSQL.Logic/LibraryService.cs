using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using NoSQL.Logic.Entities;

namespace NoSQL.Logic
{
    public class LibraryService
    {
		private MongoClient mongoClient;
		private IMongoCollection<Book> library;
		private IMongoDatabase db;
	    
		public LibraryService()
		{
			mongoClient = new MongoClient();
			db = mongoClient.GetDatabase("Library");
			library = db.GetCollection<Book>("Books");
		}

		public void AddBooks()
		{
			var listOfBooks = new List<Book>()
			{
				new Book
				{
					Name = "Hobbit" ,
					Author = "Tolkien",
					Count = 5 ,
					Genre = new List<string> { "fantasy" },
					Year = 2014
				},
				new Book
				{
					Name = "Lord of the rings" ,
					Author = "Tolkien",
					Count = 3 ,
					Genre = new List<string> { "fantasy" },
					Year = 2015
				},
				new Book
				{
					Name = "Kolobok" ,
					Count = 10 ,
					Genre = new List<string> { "kids" },
					Year = 2000
				},
				new Book
				{
					Name = "Repka",
					Count = 11,
					Genre = new List<string> { "kids" },
					Year = 2000
				},
				new Book
				{
					Name = "Dyadya Stiopa",
					Author = "Mihalkov",
					Count = 1,
					Genre = new List<string> { "kids" },
					Year = 2001
				}
			};

			library.InsertMany(listOfBooks);
			FindAll();
		}

		public void FindAll()
		{
			var books = library.AsQueryable().ToList();

			if (books.Count == 0)
			{
				Console.WriteLine("Book collection is empty");
			}

			PrintBookInfo(books);
		}

		public void FindBooksWithCountMoreThanOne()
		{
			var books = library.AsQueryable()
				.Where(book => book.Count > 1)
				.Take(3).Select(book => book.Name).ToList();

			foreach (var book in books)
			{
				Console.WriteLine($"Name: {book}.");
			}

			Console.WriteLine("-------");
			Console.WriteLine($"Count: {books.Count}.");
		}

		public void FindBooksWithMaxAndMinCount()
		{
			var maxCount = library.AsQueryable().Max(book => book.Count);
			var bookWithMaxCount = library.Find(book => true)
				.SortByDescending(book => book.Count).Limit(1).FirstOrDefault();

			Console.WriteLine($"Book with max count ({maxCount}) is {bookWithMaxCount.Name}.");

			var minCount = library.AsQueryable().Min(book => book.Count);
			var bookWithMinCount = library.Find(book => true).SortBy(book => book.Count)
				.Limit(1).FirstOrDefault();

			Console.WriteLine($"Book with min count ({minCount}) is {bookWithMinCount.Name}.");
		}

		public void FindAllAuthors()
		{
			var authors = library.AsQueryable()
				.Where(book => !string.IsNullOrEmpty(book.Author))
				.Select(book => book.Author).Distinct();

			Console.WriteLine("List of authors:");

			foreach (var author in authors)
			{
				Console.WriteLine($"Name: {author}.");
			}
		}

		public void FindBooksWithoutAuthor()
		{
			var books = library.AsQueryable()
				.Where(book => string.IsNullOrEmpty(book.Author)).ToList();

			Console.WriteLine("List of books without author:");

			PrintBookInfo(books);
		}

		public void IncreaseCountOfBookByOne()
		{
			library.UpdateMany(Builders<Book>.Filter.Empty, 
				Builders<Book>.Update.Inc("Count", 1));
			FindAll();
		}

		public void AddGenreFavorityToFantasyBook()
		{
			var filter = Builders<Book>.Filter.Where(x => x.Genre.Contains("fantasy") 
				&& !x.Genre.Contains("favority"));

			library.UpdateMany(filter,
			   Builders<Book>.Update.Set(x => x.Genre[1], "favority"));
			FindAll();
		}

		public void DeleteBooksWithCountLessThanThree()
		{
			var result = library.DeleteMany(book => book.Count < 3);

			Console.WriteLine($"{result.DeletedCount} books have been deleted.");

			FindAll();
		}

		public void DeleteAllBooks()
		{
			library.Database.DropCollection("Books");

			Console.WriteLine("All books have been deleted.");
		}

		private void PrintBookInfo(List<Book> books)
		{
			foreach (var book in books)
			{
				Console.WriteLine($"ID: {book.Id}.");
				Console.WriteLine($"Name: {book.Name}.");
				Console.WriteLine($"Author: {book.Author}.");
				Console.WriteLine($"Count: {book.Count}.");
				Console.Write($"Genre:");
				foreach (var genre in book.Genre)
				{
					Console.Write(genre + " ");
				}
				Console.WriteLine($"\nYear: {book.Year}.");
				Console.WriteLine("-------");
			}
		}
	}
}
