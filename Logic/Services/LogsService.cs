using DalFactory;
using Discord;
using IDal;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class LogsService
    {
        private readonly ILogs _logs;

        public LogsService()
        {
            _logs = LogsFactory.GenerateLogs();
        }

        public async Task Write(string name, IGuild guild, string message)
        {
            await _logs.Write(name, $"{guild.Name}({guild.Id}) - {message}");
        }

        public async Task Write(string name, string message)
        {
            await _logs.Write(name, $"{message}");
        }
    }
}
