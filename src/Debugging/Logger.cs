using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BagOfTricks.Debug
{
    public static class Logger
    {
        public static readonly string LOG_DIRECTORY = Path.Combine(Paths.PluginPath, "BagOfTricks2/Logs");
        public static string LOG_PATH = Path.Combine(LOG_DIRECTORY, $"logs {DateTime.Now:MM-dd-yyy (HH;mm)}.txt");

        public static List<LogEntry> logHistory = new();

        private static int s_LogFileThreshold = 10;

        public static void ValidatePaths()
        {
            if (!Directory.Exists(LOG_DIRECTORY))
                Directory.CreateDirectory(LOG_DIRECTORY);

            if (!File.Exists(LOG_PATH))
            {
                var fs = new FileStream(LOG_PATH, FileMode.Create);
                fs.Dispose();
            }

            RemoveOldestLogFiles();
        }

        private static void RemoveOldestLogFiles()
        {
            string[] files = Directory.GetFiles(LOG_DIRECTORY);
            if (files.Length <= s_LogFileThreshold)
                return;

            var sortedFiles = files.Select(f => new FileInfo(f))
                           .OrderBy(f => f.CreationTime)
                           .ToList();

            int filesToRemove = sortedFiles.Count - s_LogFileThreshold;
            for (int i = 0; i < filesToRemove; i++)
            {
                File.Delete(sortedFiles[i].FullName);
            }
        }

        public static void Write<T>(object message, Exception exception = null) where T : ILoggable
        {
            string logMessage = message == null ? "NULL" : message.ToString();

            Color logColor = Color.white;
            if (typeof(T) == typeof(Info))
            {
                ParseInfo(ref logMessage);
            }
            else if (typeof(T) == typeof(Warning))
            {
                logColor = Styles.Colors.WarningYellow;
                ParseWarning(ref logMessage);
            }
            else if (typeof(T) == typeof(Error))
            {
                logColor = Styles.Colors.ErrorRed;
                ParseError(ref logMessage, exception);
            }
            else if (typeof(T) == typeof(Success))
            {
                logColor = Styles.Colors.SuccessGreen;
                ParseSuccess(ref logMessage);
            }

            try
            {
                using StreamWriter sw = File.AppendText(LOG_PATH);
                sw.WriteLine(logMessage);

                Plugin.UnhandledLogs.Enqueue(logMessage);

                var entry = new LogEntry(logColor, AddRichTextTo(logMessage));
                logHistory.Add(entry);
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Error writing to log: {e.Message}");
            }
        }

        private static void ParseError(ref string errorString, Exception e)
        {
            DateTime dt = DateTime.Now;
            errorString = $"[Error] {dt:MM/dd/yyyy HH:mm} - " + errorString;
            if (e != null)
            {
                errorString += "\nException info: " + e;
            }
        }

        private static void ParseWarning(ref string warningString)
        {
            DateTime dt = DateTime.Now;
            warningString = $"[Warning] {dt:MM/dd/yyyy HH:mm} - " + warningString;
        }

        private static void ParseInfo(ref string infoString)
        {
            DateTime dt = DateTime.Now;
            infoString = $"[Info] {dt:MM/dd/yyyy HH:mm} - " + infoString;
        }

        private static void ParseSuccess(ref string successString)
        {
            DateTime dt = DateTime.Now;
            successString = $"[Success] {dt:MM/dd/yyyy HH:mm} - " + successString;
        }

        private static string AddRichTextTo(string logMessage)
        {
            int firstSpaceIndex = logMessage.IndexOf(' ');
            int secondSpaceIndex = logMessage.IndexOf(' ', firstSpaceIndex + 1);
            int thirdSpaceIndex = logMessage.IndexOf(' ', secondSpaceIndex + 1);

            string formattedMessage = "<b>" + logMessage.Substring(0, thirdSpaceIndex) + "</b>" + logMessage.Substring(thirdSpaceIndex);
            return formattedMessage;
        }

    }

    public struct Info : ILoggable { }

    public struct Warning : ILoggable { }

    public struct Error : ILoggable { }

    public struct Success : ILoggable { }

    public interface ILoggable { }

    public struct LogEntry
    {
        public Color logColor;
        public string logMessage;

        public LogEntry(Color logColor, string logMessage) 
        { 
            this.logColor = logColor; 
            this.logMessage = logMessage; 
        }
    }
}
