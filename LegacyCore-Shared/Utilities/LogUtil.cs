﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace LegacyCore.Utilities
{
    /// <summary>
    /// Logs info, warning and error messages to both the console and optional file.
    /// </summary>
    internal static class LogUtil
    {
        /// <summary>
        /// If set, the <see cref="LogUtil"/> will also output to a file.
        /// </summary>
        public static string filePath;

        private static DateTime _startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();

        private static void Write(string value)
        {
            Console.WriteLine(value);

            if (!string.IsNullOrEmpty(filePath))
                File.AppendAllText(filePath, $"{value}\r\n");
        }

        private static void LogInternal(ConsoleColor color, string type, string message, string memberName)
        {
            Console.ForegroundColor = color;

            foreach (var value in message.Split('\n'))
            {
                Write($"[{DateTime.UtcNow - _startTime}, {type}] | {memberName} > {value}");
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="memberName">The sender of the message.</param>
        public static void Info(string message, [CallerMemberName] string memberName = "")
            => LogInternal(ConsoleColor.White, "Info", message, memberName);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="memberName">The sender of the message.</param>
        public static void Warning(string message, [CallerMemberName] string memberName = "")
            => LogInternal(ConsoleColor.Yellow, "Warning", message, memberName);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="memberName">The sender of the message.</param>
        public static void Error(string message, [CallerMemberName] string memberName = "")
            => LogInternal(ConsoleColor.Red, "Error", message, memberName);

        /// <summary>
        /// Attempt to run an action, if an <see cref="Exception"/> occurs
        /// then it logs the error.
        /// </summary>
        /// <param name="action">The action to attempt.</param>
        public static void Attempt(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Error(e.ToString(), "Exception");
            }
        }
    }
}
