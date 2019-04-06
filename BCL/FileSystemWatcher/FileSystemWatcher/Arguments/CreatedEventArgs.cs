using System;

namespace FileSystemWatcher.Arguments
{
    public class CreatedEventArgs<TModel> : EventArgs
    {
        public TModel CreatedItem { get; set; }
    }
}
