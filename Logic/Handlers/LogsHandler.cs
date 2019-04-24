using DalFactory;
using Discord;
using IDal.Interfaces;

namespace Logic.Handlers
{
    public class LogsHandler
    {
        private static LogsHandler _instance;

        private readonly ILogs _logs;

        public LogsHandler()
        {
            _logs = LogsFactory.GenerateLogs();
        }

        public static LogsHandler Instance => _instance ?? (_instance = new LogsHandler());

        public void Log(string name, IGuild guild, string message)
        {
            _logs.Log(name, $"{guild.Name}({guild.Id}) - {message}");
        }

        public void Log(string name, string message)
        {
            _logs.Log(name, $"{message}");
        }
    }
}