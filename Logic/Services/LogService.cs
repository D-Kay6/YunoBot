using DalFactory;
using Discord;
using IDal.Interfaces;

namespace Logic.Services
{
    public class LogService
    {
        private static LogService _instance;

        public static LogService Instance => _instance ?? (_instance = new LogService());

        private readonly ILogs _logs;

        public LogService()
        {
            _logs = LogsFactory.GenerateLogs();

            if (_instance == null) _instance = this;
        }

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
