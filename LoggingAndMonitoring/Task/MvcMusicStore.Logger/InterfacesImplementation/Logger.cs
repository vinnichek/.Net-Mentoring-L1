using System;
using NLog;

namespace MvcMusicStore.Logger.InterfacesImplementation
{
    public class Logger : Interfaces.ILogger
    {
        private readonly ILogger logger = LogManager.GetLogger("MvcMusicStoreLogger");

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

		public void Error(string message, Exception ex)
		{
			logger.Error(ex, message);
		}
	}
}
