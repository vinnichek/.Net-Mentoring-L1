using FileSystemWatcher.Interfaces;
using System;

namespace FileSystemWatcher.InterfacesImplementation
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
