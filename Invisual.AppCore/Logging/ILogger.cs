using System;

namespace Invisual.AppCore.Logging
{
	public interface ILogger
	{
		void Log(LogLevel level, string message);
		void Log(LogLevel level, string message, Exception exception);
	}
}
