using System;
using System.ComponentModel;
using System.IO;

namespace Invisual.AppCore.Logging
{
	/// <summary>
	/// Basic <see cref="ILogger"/> which will write messages to the specified <see cref="TextWriter"/>.
	/// </summary>
	/// <example>
	/// // All messages logged at LogLevel.Info level and above will be written to the Console.
	/// var logger = new TextWriterLogger(Console.Out, LogLevel.Info);
	///
	///	logger.Log(LogLevel.Error, "Error message");
	///	logger.Log(LogLevel.Warning, "Warning message");
	///	logger.Log(LogLevel.Info, "Info message");
	///	logger.Log(LogLevel.Debug, "Debug message");
	///	logger.Log(LogLevel.Trace, "Trace message");
	/// </example>
	public class TextWriterLogger : ILogger
	{
		private readonly TextWriter _writer;
		private readonly LogLevel _minLevel;

		public TextWriterLogger(TextWriter writer, LogLevel minLevel)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (!Enum.IsDefined(typeof(LogLevel), minLevel))
				throw new InvalidEnumArgumentException("minLevel", (int)minLevel, typeof(LogLevel));

			_writer = writer;
			_minLevel = minLevel;
		}

		/// <summary>
		/// Log a message at the specified <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="level"></param>
		/// <param name="message"></param>
		public void Log(LogLevel level, string message)
		{
			if (!Enum.IsDefined(typeof(LogLevel), level))
				throw new InvalidEnumArgumentException("level", (int)level, typeof(LogLevel));

			if (level >= _minLevel)
				_writer.WriteLine(message);
		}

		public void Log(LogLevel level, string message, Exception exception)
		{
			if (!Enum.IsDefined(typeof(LogLevel), level))
				throw new InvalidEnumArgumentException("level", (int)level, typeof(LogLevel));

			if (level < _minLevel)
				return;

			_writer.WriteLine(message);

			if (exception != null)
				_writer.WriteLine(exception);
		}
	}
}
