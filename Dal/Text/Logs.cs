using IDal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dal.Text
{
    public class Logs : ILogs
    {
        private readonly TextStream _stream = new TextStream();

        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Logs");

        public async Task Write(string file, string message)
        {
            try
            {
                var directory = Path.Combine(Directory, file);
                var path = Path.Combine(directory, $"{DateTime.Today:yy-MM-dd}.txt");
                if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
                await _stream.WriteLineAsync(path, $"[{DateTime.Now.ToShortTimeString()}] {message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Failed to write log {file}.\n{message}.\n{e}.");
            }
        }
    }
}