using DalFactory;
using Discord;
using IDal.Interfaces;

namespace Logic.Services.Logs
{
    public class LogService
    {
        private readonly ILogs _logs;

        public LogService()
        {
            _logs = LogsFactory.GenerateLogs();
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
