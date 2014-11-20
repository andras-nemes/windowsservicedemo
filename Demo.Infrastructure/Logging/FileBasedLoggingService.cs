using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Logging
{
	public class FileBasedLoggingService : ILoggingService
	{
		private readonly string _logFileFullPath;

		public FileBasedLoggingService(string logFileFullPath)
		{
			if (string.IsNullOrEmpty(logFileFullPath)) throw new ArgumentException("Log file full path");
			FileInfo logFileInfo = new FileInfo(logFileFullPath);
			if (!logFileInfo.Exists) throw new ArgumentNullException("Log file does not exist");
			_logFileFullPath = logFileFullPath;
		}

		public void LogInfo(object logSource, string message, Exception exception = null)
		{
			AppendMessage(logSource, message, "INFO", exception);
		}

		public void LogWarning(object logSource, string message, Exception exception = null)
		{
			AppendMessage(logSource, message, "WARNING", exception);
		}

		public void LogError(object logSource, string message, Exception exception = null)
		{
			AppendMessage(logSource, message, "ERROR", exception);
		}

		public void LogFatal(object logSource, string message, Exception exception = null)
		{
			AppendMessage(logSource, message, "FATAL", exception);
		}

		private void AppendMessage(object source, string message, string level, Exception exception)
		{
			try
			{
				File.AppendAllText(_logFileFullPath, FormatMessage(source, message, level, exception));
			}
			catch
			{}
		}

		private string FormatMessage(object source, string message, string level, Exception exception)
		{
			return string.Concat(Environment.NewLine, DateTime.UtcNow.ToString(), ": source: ", source.ToString(), ", level: ", level, ", message: "
				, message, ", any exception: ", (exception == null ? "None" : exception.Message) );
		}
	}
}
