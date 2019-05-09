using WGETAnalog.ConsoleApp.Restrictions;
using WGETAnalog.Logic.Interfaces;
using WGETAnalog.Logic.InterfacesImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WGETAnalog.Logic.Enums;

namespace WGETAnalog.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = new DirectoryInfo("/Users/vinnichek/2");
            var saver = new Saver(directory);
            var restrictions = new List<IRestrictionHelper>();
            var logger = new Logger();

            restrictions.Add(new FileRestrictionHelper("js".Select(r => "." + r)));
            restrictions.Add(new DomainRestrictionHelper(DomainRestriction.All, new Uri("https://tvoy-halat.by")));

            var downloader = new Logic.InterfacesImplementation.WGETAnalog(saver, restrictions, logger, 1);

            try
            {
                downloader.DownloadSite("https://tvoy-halat.by");
            }

            catch (Exception ex)
            {
                logger.Log($"Some error occured during site downloading: {ex.Message}");
            }
        }
    }
}