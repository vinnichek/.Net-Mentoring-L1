using FileSystemWatcher.Arguments;
using FileSystemWatcher.Configuration;
using FileSystemWatcher.Interfaces;
using FileSystemWatcher.InterfacesImplementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher
{
    class Program
    {
        private static FileSystemWatcher listener;
        private static List<string> directories;
        private static List<RuleElement> rules;

        static void Main(string[] args)
        {
            ILogger logger = new Logger();
            FileSystemWatcherSection configuration = ConfigurationManager
                .GetSection("fileSystemSection") as FileSystemWatcherSection;

            if (configuration == null)
            {
                logger.Log(ResourcesString.ConfigurationNotFound);
                return;
            }

            GetConfiguration(configuration);

            logger.Log(ResourcesString.CurrentCulture);

            listener = new FileSystemWatcher(rules, configuration.Rules.DefaultDirectory, logger);
            var service = new FileSystemWatcherService(directories, logger);

            service.FileCreated += OnCreated;

            logger.Log(ResourcesString.Exit);
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
        }

        private static void GetConfiguration(FileSystemWatcherSection configuration)
        {
            directories = new List<string>(configuration.Directories.Count);
            rules = new List<RuleElement>();

            foreach (DirectoryElement directory in configuration.Directories)
            {
                directories.Add(directory.DirectoryPath);
            }

            foreach (RuleElement rule in configuration.Rules)
            {
                rules.Add(rule);
            }

            CultureInfo.DefaultThreadCurrentCulture = configuration.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = configuration.Culture;
        }

        private static void OnCreated(object sender, CreatedEventArgs<FileInfo> args)
        {
            listener.MoveItem(args.CreatedItem);
        }
    }
}
