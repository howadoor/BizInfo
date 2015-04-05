using System;
using System.Diagnostics;
using System.Text;
using Common.Logging;
using Perenis.Core.Pattern;

namespace Perenis.Core.Log
{
    /// <summary>
    /// Provides extension methods for logging messages into the configuration-defined loggers.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// this.Log(LogLevel, exception, message);
    /// ]]>
    /// </code>
    /// </example>
    public static class Logging
    {
        public static bool IsLoggingEnabled(this object source, LogLevel level)
        {
            Debug.Assert(source != null);

            ILog logger = LogManager.GetLogger(source.GetType());

            switch (level)
            {
                case LogLevel.Trace:
                    return logger.IsTraceEnabled;
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Info:
                    return logger.IsInfoEnabled;
                case LogLevel.Warn:
                    return logger.IsWarnEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return logger.IsFatalEnabled;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }

        public static void LogDebug(this object source, string message)
        {
            Log(source, LogLevel.Debug, null, message);
        }

        public static void LogDebug(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Debug, null, String.Format(message, args));
        }

        public static void LogDebug(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Debug, exception, message);
        }

        public static void LogDebug(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Debug, exception, String.Format(message, args));
        }

        public static void LogError(this object source, string message)
        {
            Log(source, LogLevel.Error, null, message);
        }

        public static void LogError(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Error, null, String.Format(message, args));
        }

        public static void LogError(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Error, exception, message);
        }

        public static void LogError(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Error, exception, String.Format(message, args));
        }

        public static void LogFatal(this object source, string message)
        {
            Log(source, LogLevel.Fatal, null, message);
        }

        public static void LogFatal(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Fatal, null, String.Format(message, args));
        }

        public static void LogFatal(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Fatal, exception, message);
        }

        public static void LogFatal(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Fatal, exception, String.Format(message, args));
        }

        public static void LogInfo(this object source, string message)
        {
            Log(source, LogLevel.Info, null, message);
        }

        public static void LogInfo(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Info, null, String.Format(message, args));
        }

        public static void LogInfo(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Info, exception, message);
        }

        public static void LogInfo(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Info, exception, String.Format(message, args));
        }

        public static void LogTrace(this object source, string message)
        {
            Log(source, LogLevel.Trace, null, message);
        }

        public static void LogTrace(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Trace, null, String.Format(message, args));
        }

        public static void LogTrace(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Trace, exception, message);
        }

        public static void LogTrace(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Trace, exception, String.Format(message, args));
        }

        public static void LogWarn(this object source, string message)
        {
            Log(source, LogLevel.Warn, null, message);
        }

        public static void LogWarn(this object source, string message, params object[] args)
        {
            Log(source, LogLevel.Warn, null, String.Format(message, args));
        }

        public static void LogWarn(this object source, Exception exception, string message)
        {
            Log(source, LogLevel.Warn, exception, message);
        }

        public static void LogWarn(this object source, Exception exception, string message, params object[] args)
        {
            Log(source, LogLevel.Warn, exception, String.Format(message, args));
        }

        public static void Log(this object source, LogLevel logLevel, string message)
        {
            Log(source, logLevel, null, message);
        }

        public static void Log(this object source, LogLevel logLevel, string message, params object[] args)
        {
            Log(source, logLevel, null, String.Format(message, args));
        }

        public static void Log(this object source, LogLevel logLevel, Exception exception, string message)
        {
            Debug.Assert(source != null);
            Debug.Assert(message != null);

            var sb = new StringBuilder(message.Length + 20);
            sb.Append(LoggingContextProvider.CurrentLoggingSessionId);
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(message);
            if (exception != null)
            {
                char c = sb.Length > 0 ? sb[sb.Length - 1] : ' ';
                if (c != '.' && c != ';') sb.Append(';');
                sb.Append(' ');
                sb.Append(LogException(source, logLevel, exception));
            }
            message = sb.ToString();

            ILog logger = LogManager.GetLogger(source.GetType());
            switch (logLevel)
            {
                case LogLevel.Debug:
                    logger.Debug(message);
                    break;
                case LogLevel.Error:
                    logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    logger.Fatal(message);
                    break;
                case LogLevel.Info:
                    logger.Info(message);
                    break;
                case LogLevel.Trace:
                    logger.Trace(message);
                    break;
                case LogLevel.Warn:
                    logger.Warn(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        public static void Log(this object source, LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            Log(source, logLevel, exception, String.Format(message, args));
        }

        public static string LogException(this object source, LogLevel level, Exception ex)
        {
            return Singleton<ExceptionLogger>.Instance.LogException(source, level, ex);
        }

        public static string LogDump(this object source, LogLevel level, string data)
        {
            return Singleton<DataLogger>.Instance.LogDump(source, level, data);
        }

        public static string LogDumpShort(this object source, LogLevel level, string data)
        {
            return Singleton<DataLogger>.Instance.LogDumpShort(source, level, data);
        }
    }
}