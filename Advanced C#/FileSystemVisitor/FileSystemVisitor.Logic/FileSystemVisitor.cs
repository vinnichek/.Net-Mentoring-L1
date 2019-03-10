using System;
using System.Collections.Generic;
using System.IO;
using FileSystem.Logic;
using FileSystem.Logic.Arguments;

namespace FileSystem.Logic
{
	public class FileSystemVisitor
	{
		private readonly Func<FileSystemInfo, bool> Filter;

		public event EventHandler<EventArgs> OnStart;
		public event EventHandler<EventArgs> OnFinish;

		public event EventHandler<FileSystemItemArgs> OnFileFound;
		public event EventHandler<FileSystemItemArgs> OnDirectoryFound;
		public event EventHandler<FileSystemItemArgs> OnFilteredFileFound;
		public event EventHandler<FileSystemItemArgs> OnFilteredDirectoryFound;

		public FileSystemVisitor(Func<FileSystemInfo, bool> filter = null)
		{
			Filter = filter ?? ((FileSystemInfo startDirectory) => false);
		}

		public IEnumerable<FileSystemInfo> VisitDirectory(string startDirectory)
		{
			if (string.IsNullOrEmpty(startDirectory))
			{
				throw new ArgumentNullException($"{nameof(startDirectory)} is null or empty.");
			}

			if (!Directory.Exists(startDirectory))
			{
				throw new ArgumentException($"{nameof(startDirectory)} is not exist.");
			}

			OnEvent(OnStart, new EventArgs());

			DirectoryInfo info = new DirectoryInfo(startDirectory);
			FileSystemInfo[] foundFilesAndDirectories = info.GetFileSystemInfos("*", SearchOption.AllDirectories);

			foreach (FileSystemInfo item in ProcessItems(foundFilesAndDirectories))
			{
				yield return item;
			}

			OnEvent(OnFinish, new EventArgs());
		}

		private IEnumerable<FileSystemInfo> ProcessItems(FileSystemInfo[] items)
		{
			foreach (var item in items)
			{
				ActionType action;

				if (item is DirectoryInfo)
				{
					action = ProcessItem(item, item.Name, OnDirectoryFound, OnFilteredDirectoryFound);
				}

				else
				{
					action = ProcessItem(item, item.Name, OnFileFound, OnFilteredFileFound);
				}

				switch (action)
				{
					case ActionType.Stop:
						{
							yield break;
						}
					case ActionType.Continue:
						{
							yield return item;
							break;
						}
				}
			}
		}

		private ActionType ProcessItem(FileSystemInfo entry, string entryName,
			EventHandler<FileSystemItemArgs> itemFinded,
			EventHandler<FileSystemItemArgs> itemFiltered)
		{
			var args = new FileSystemItemArgs(entryName);
			OnEvent(itemFinded, args);

			if (args.Action != ActionType.Continue)
			{
				return args.Action;
			}

			if (!Filter(entry))
			{
				OnEvent(itemFiltered, args);
				return args.Action;
			}

			return ActionType.Skip;
		}

		private void OnEvent<T>(EventHandler<T> eventHandler, T args)
		{
			eventHandler?.Invoke(this, args);
		}
	}
}
