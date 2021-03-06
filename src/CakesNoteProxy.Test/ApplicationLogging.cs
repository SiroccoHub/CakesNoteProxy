﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CakesNoteProxy.Test
{
    public static class ApplicationLogging
    {
        private static bool _initialized;

        private static ILoggerFactory _loggerFactory;
        public static ILoggerFactory LoggerFactory => LoggerFactoryInternal();

        private static ILoggerFactory LoggerFactoryInternal()
        {
            if (_initialized)
                return _loggerFactory;

            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddConsole(LogLevel.Debug);
            _loggerFactory.AddDebug();
            _initialized = true;

            return _loggerFactory;
        }

        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}
