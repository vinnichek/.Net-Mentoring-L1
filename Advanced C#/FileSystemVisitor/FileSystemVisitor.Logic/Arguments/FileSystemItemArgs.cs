using System;

namespace FileSystem.Logic.Arguments
{
	public class FileSystemItemArgs : EventArgs
	{
		public ActionType Action { get; set; }

		public string ItemName { get; internal set; }

		public FileSystemItemArgs(string itemName, ActionType action = ActionType.Continue)
		{
			ItemName = itemName;
			Action = action;
		}
	}
}
