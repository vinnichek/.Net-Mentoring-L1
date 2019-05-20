using System;

namespace MvcMusicStore.Logger.Interfaces
{
    public interface ILogger
    {
        void Trace(string message);

        void Info(string message);

        void Debug(string message);

        void Error(string message);

		void Error(string message, Exception ex);
	}
}
