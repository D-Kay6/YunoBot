﻿using Discord;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Factory;

namespace Yuno.Main.Logging
{
    public class LogsHandler
    {
        private static LogsHandler _instance;

        public static LogsHandler Instance
        {
            get { return _instance ?? (_instance = new LogsHandler()); }
        }

        private ILogs _logs;

        public LogsHandler()
        {
            this._logs = LogsFactory.GenerateLogs();
        }

        public void Log(string name, IGuild guild, string message)
        {
            _logs.Log(name, $"{guild.Id} - {message}");
        }

        public void Log(string name, string message)
        {
            _logs.Log(name, $"{message}");
        }
    }
}
