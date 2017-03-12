﻿using UnityEngine;

/***
 * Log.cs
 *
 * @author administrator
 */
namespace GameEngine
{
    public static class Log
    {
        private static int stackFrameIndex = 7;

        private static LoggerType[] toLogLevel = {
            LoggerType.Error, // LogType.Error
            LoggerType.Error, // LogType.Assert
            LoggerType.Warn,  // LogType.Warn
            LoggerType.Info,  // LogType.Log
            LoggerType.Error, // LogType.Exception
        };

        private static ILogger logger = null;

        static Log()
        {
            if (logger == null) {
                logger = new Logger(stackFrameIndex);
                InitLogCallback();
            }
        }

        private static void InitLogCallback()
        {
#if UNITY_4
            Application.RegisterLogCallback(OnLogHandle);
#else
            Application.logMessageReceived += OnLoggerHandle;
#endif
        }

        private static void OnLoggerHandle(string condition, string stackTrace, LogType type)
        {
            if (logger == null) {
                return;
            }
            logger.Log(toLogLevel[(int)type], condition, stackTrace);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Assert(bool condition)
        {
            if (logger != null) {
                logger.Assert(condition, string.Empty, true);
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Assert(bool condition, string assertString)
        {
            if (logger != null) {
                logger.Assert(condition, assertString, true);
            }
        }

        public static void Info(string msg, params object[] args)
        {
            if (logger != null) {
                logger.Info(msg, args);
            }
        }

        public static void Warn(string msg, params object[] args)
        {
            if (logger != null) {
                logger.Warn(msg, args);
            }
        }

        public static void Error(string msg, params object[] args)
        {
            if (logger != null) {
                logger.Error(msg, args);
            }
        }

        public static void Exception(System.Exception ex)
        {
            if (logger != null) {
                logger.Exception(ex);
            }
        }

        public static void Dispose()
        {
            if (logger != null) {
                logger.Dispose();
                logger = null;
            }
#if UNITY_5
            Application.logMessageReceived -= OnLogHandle;
#endif
        }
    }
}