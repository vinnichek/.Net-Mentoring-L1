using FileSystem.Logic;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Tests
{
	public class Tests
	{
		[TestFixture]
		public class FileSystemVisitorTest
		{
			private FileSystemVisitor fileSystemVisitor;

			[SetUp]
			public void Initialize()
			{
				fileSystemVisitor = new FileSystemVisitor();
			}

			[Test]
			public void VisitDirectory_NullPath_ThrowArgumentNullException()
			{
				string path = null;

				Assert.Throws<ArgumentNullException>(() =>
					fileSystemVisitor.VisitDirectory(path).ToList());
			}

			[Test]
			public void VisitDirectory_NotValidPath_ThrowArgumentException()
			{
				string path = "S://";

				Assert.Throws<ArgumentException>(() =>
					fileSystemVisitor.VisitDirectory(path).ToList());
			}

			[Test]
			public void VisitDirectory_EventsCalls_EventsAreCalledOrNot()
			{
				string path = "D://Test";
				bool startEventCall = false, finishEventCall = false,
					directoryFoundEventCall = false, fileFoundEventCall = false,
					filteredDirectoryFoundEventCall = false, filteredFileFoundEventCall = false;

				fileSystemVisitor.OnStart += (s, e) => startEventCall = true;
				fileSystemVisitor.OnFinish += (s, e) => finishEventCall = true;
				fileSystemVisitor.OnDirectoryFound += (s, e) => directoryFoundEventCall = true;
				fileSystemVisitor.OnFilteredDirectoryFound += (s, e) => filteredFileFoundEventCall = true;

				fileSystemVisitor.VisitDirectory(path).ToList();

				Assert.IsTrue(startEventCall);
				Assert.IsTrue(finishEventCall);
				Assert.IsTrue(directoryFoundEventCall);
				Assert.IsFalse(fileFoundEventCall);
				Assert.IsFalse(filteredDirectoryFoundEventCall);
				Assert.IsTrue(filteredFileFoundEventCall);
			}

			[Test]
			public void VisitDirectory_ActionTypeIsSkip_SkipNeededFile()
			{
				string path = "D://Test/tri-css-server-master";

				fileSystemVisitor.OnFileFound += (s, e) =>
				{
					if (e.ItemName.Contains("cli"))
					{
						e.Action = ActionType.Skip;
					}
				};

				var allItems = Directory.GetFileSystemEntries(path).ToList();
				var afterFilterItems = fileSystemVisitor.VisitDirectory(path).ToList();

				Assert.AreEqual(allItems.Count - 1, afterFilterItems.Count);
			}

			[Test]
			public void VisitDirectory_ActionTypeIsStop_StopVisit()
			{
				string path = "D://Test/tri-css-server-master";

				fileSystemVisitor.OnFileFound += (s, e) =>
				{
					if (e.ItemName.Contains("cli"))
					{
						e.Action = ActionType.Stop;
					}
				};

				var allItems = Directory.GetFileSystemEntries(path).ToList();
				var result = fileSystemVisitor.VisitDirectory(path).ToList();

				Assert.AreNotEqual(allItems.Count, result.Count);
			}
		}
	}
}