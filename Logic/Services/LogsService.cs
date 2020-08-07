using DalFactory;
using Discord;
using IDal;
using System;
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

        public async Task Write(string name, string message, IGuild guild = null)
        {
            if (guild != null)
                message = $"{guild.Name}({guild.Id}) - {message}";

            await _logs.Write(name, $"{message}");
        }

        public async Task Write(string name, Exception exception, IGuild guild = null)
        {
            var message = exception.Message;
            if (guild != null)
                message = $"{guild.Name}({guild.Id}) - {message}";

            var stacktrace = exception.StackTrace;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                message += $"\r\n   {exception.Message}";
            }
            message += $"\r\n  {stacktrace}";

            await _logs.Write(name, message);
        }

        public async Task Write(string name, string message, Exception exception, IGuild guild = null)
        {
            if (guild != null)
                message = $"{guild.Name}({guild.Id}) - {message}";

            var stacktrace = exception.StackTrace;
            do
            {
                message += $"\r\n   {exception.Message}";
                exception = exception.InnerException;
            } while (exception != null);
            message += $"\r\n  {stacktrace}";

            await _logs.Write(name, message);
        }
    }
}