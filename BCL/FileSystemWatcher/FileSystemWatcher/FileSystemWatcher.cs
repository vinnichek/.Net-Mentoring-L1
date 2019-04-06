using FileSystemWatcher.Configuration;
using FileSystemWatcher.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ResourcesString = FileSystemWatcher.Resources.Resources;

namespace FileSystemWatcher
{
    public class FileSystemWatcher
    {
        private readonly ILogger logger;
        private readonly List<RuleElement> rules;
        private readonly string defaultDirectory;

        public FileSystemWatcher(List<RuleElement> rules, string defaultDirectory, ILogger logger)
        {
            this.rules = rules;
            this.defaultDirectory = defaultDirectory;
            this.logger = logger;
        }

        public void MoveItem(FileInfo file)
        {
            int matchCount = 0;
            string itemPath = file.FullName;

            foreach (var rule in rules)
            {
                var template = new Regex(rule.Template);
                var isMatch = template.IsMatch(file.Name);

                if (isMatch)
                {
                    matchCount++;
                    logger.Log(ResourcesString.RuleMatch);
                    string destinationPath = CreateDestinationPath(file, rule, matchCount);
                    Move(itemPath, destinationPath);
                    logger.Log(string.Format(ResourcesString.FileMoved, file.FullName, destinationPath));
                    return;
                }
            }

            string defaultPath = Path.Combine(defaultDirectory, file.Name);
            logger.Log(ResourcesString.RuleNotMatch);
            Move(itemPath, defaultPath);
            logger.Log(string.Format(ResourcesString.FileMoved, file.FullName, defaultPath));
        }

        private string CreateDestinationPath(FileInfo file, RuleElement rule, int matchCount)
        {
            string extension = Path.GetExtension(file.Name);
            string filename = Path.GetFileNameWithoutExtension(file.Name);

            var destinationPath = new StringBuilder()
                .Append(Path.Combine(rule.DestinationDirectory, filename));

            if (rule.IsDateRequired)
            {
                var format = CultureInfo.CurrentCulture.DateTimeFormat;
                format.DateSeparator = ".";
                destinationPath.Append($"_{DateTime.Now.ToLocalTime().ToString(format.ShortDatePattern)}");
            }

            if (rule.IsIndexNumberRequired)
            {
                destinationPath.Append($"_{matchCount}");
            }

            return destinationPath.Append(extension).ToString();
        }

        private void Move(string itemPath, string destinationPath)
        {
            string dir = Path.GetDirectoryName(destinationPath);
            bool fileAcess = true;

            Directory.CreateDirectory(dir);
            do
            {
                try
                {
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Move(itemPath, destinationPath);
                    fileAcess = false;
                }
                catch (FileNotFoundException)
                {
                    logger.Log(ResourcesString.FileNotFound);
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(2000);
                }
            } while (fileAcess);
        }
    }
}
