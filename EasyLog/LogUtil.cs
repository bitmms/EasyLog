using System;
using System.Collections.Generic;

namespace EasyLog
{
    /// <summary>
    /// 日志工具类
    /// </summary>
    public static class LogUtil
    {
        /// <summary>
        /// 定义日志类型枚举，区分不同级别的日志
        /// </summary>
        public enum LogLevel
        {
            Info,
            Warn,
            Error
        }

        /// <summary>
        /// 定义日志处理委托，用于封装日志输出的行为
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="logLevel">日志级别</param>
        public delegate void LogHandler(string text, LogLevel logLevel);

        /// <summary>
        /// 日志类型与控制台输出颜色的映射关系
        /// </summary>
        private static readonly Dictionary<LogLevel, ConsoleColor> LogLevelAndColorMap = new Dictionary<LogLevel, ConsoleColor>
        {
            { LogLevel.Info, ConsoleColor.Green },
            { LogLevel.Warn, ConsoleColor.Yellow },
            { LogLevel.Error, ConsoleColor.Red },
        };

        /// <summary>
        /// 默认日志处理器
        /// </summary>
        private static readonly LogHandler DefaultLogHandler = (text, logLevel) =>
        {
            if (text == null)
            {
                text = "";
            }

            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                if (LogLevelAndColorMap.TryGetValue(logLevel, out var value))
                {
                    Console.ForegroundColor = value;
                }

                string logFormatString = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {$"[{logLevel.ToString()}]",-7} {text}";
                Console.WriteLine(logFormatString);
            }
            catch
            {
                // ignored
            }
            finally
            {
                try
                {
                    Console.ForegroundColor = originalColor;
                }
                catch
                {
                    // ignored
                }
            }
        };

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object SLock = new object();

        /// <summary>
        /// 当前生效的日志处理委托
        /// </summary>
        private static LogHandler _logHandler = DefaultLogHandler;

        /// <summary>
        /// 内部的日志记录方法，负责加锁并调用当前生效的日志委托【线程安全】
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="logLevel">日志级别</param>
        private static void WriteLogCore(string text, LogLevel logLevel)
        {
            // 加锁目的：安全的执行委托
            lock (SLock)
            {
                _logHandler?.Invoke(text, logLevel);
            }
        }

        /// <summary>
        /// 设置当前生效的日志处理委托【线程安全】
        /// </summary>
        /// <param name="logHandler">日志处理委托</param>
        public static void SetCurrentLogHandler(LogHandler logHandler)
        {
            if (logHandler == null)
            {
                return;
            }

            // 加锁目的：安全的更新委托
            lock (SLock)
            {
                _logHandler = logHandler;
            }
        }

        /// <summary>
        /// 输出 Info 日志
        /// </summary>
        /// <param name="text">待输出的日志文本</param>
        public static void Info(string text) => WriteLogCore(text, LogLevel.Info);

        /// <summary>
        /// 输出 Warn 日志
        /// </summary>
        /// <param name="text">待输出的日志文本</param>
        public static void Warn(string text) => WriteLogCore(text, LogLevel.Warn);

        /// <summary>
        /// 输出 Error 日志
        /// </summary>
        /// <param name="text">待输出的日志文本</param>
        public static void Error(string text) => WriteLogCore(text, LogLevel.Error);
    }
}