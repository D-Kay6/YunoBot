using System;
using System.IO;
using IDal.Interfaces;

namespace Dal.Text
{
    public class Logs : ILogs
    {
        private const string Directory = "Logs";

        private TextStream _stream = new TextStream();

        public void Log(string file, string data)
        {
            try
            {
                var directory = Path.Combine(Directory, file);
                var path = Path.Combine(directory, $"{DateTime.Today:yy-MM-dd}.txt");
                if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
                _stream.WriteLine(path, $"[{DateTime.Now.ToShortTimeString()}] {data}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Failed to write log {file}.\n{data}.\n{e}.");
            }
        }
    }
}
