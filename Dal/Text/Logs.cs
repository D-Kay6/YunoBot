using System;
using System.IO;
using System.Threading.Tasks;
using IDal;

namespace Dal.Text
{
    public class Logs : ILogs
    {
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Logs");

        private readonly TextStream _stream = new TextStream();

        public async Task Write(string file, string data)
        {
            try
            {
                var directory = Path.Combine(Directory, file);
                var path = Path.Combine(directory, $"{DateTime.Today:yy-MM-dd}.txt");
                if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
                await _stream.WriteLineAsync(path, $"[{DateTime.Now.ToShortTimeString()}] {data}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Failed to write log {file}.\n{data}.\n{e}.");
            }
        }
    }
}
