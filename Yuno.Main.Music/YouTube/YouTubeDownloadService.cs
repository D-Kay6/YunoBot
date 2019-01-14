using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Yuno.Main.Music.YouTube
{
    public class YouTubeDownloadService
    {
        private const string Directory = "Data/Songs";

        // --download-archive \"{Directory}/Archive.txt\"
        private string DownloadParams => $"-o \"{Directory}/%(id)s.mp3\" --restrict-filenames --no-overwrites --extract-audio --print-json --audio-format mp3 --ignore-errors";

        public async Task<YoutubeVideo> DownloadUrl(string url)
        {
            var youtubeDl = StartYoutubeDl($"{DownloadParams} \"{url}\"");

            if (youtubeDl == null)
            {
                Console.WriteLine("Error: Unable to start process");
                return null;
            }

            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");
            return JsonConvert.DeserializeObject<YoutubeVideo>(jsonOutput);
        }

        public async Task<YoutubeVideo> DownloadSearch(string name)
        {
            var youtubeDl = StartYoutubeDl($"{DownloadParams} \"ytsearch:{name}\"");

            if (youtubeDl == null)
            {
                Console.WriteLine("Error: Unable to start process");
                return null;
            }

            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");
            return JsonConvert.DeserializeObject<YoutubeVideo>(jsonOutput);
        }

        public async Task<List<YoutubeVideo>> DownloadPlaylist(string url)
        {
            var youtubeDl = StartYoutubeDl($"{DownloadParams} {url}");

            if (youtubeDl == null)
            {
                Console.WriteLine("Error: Unable to start process");
                return null;
            }

            var videos = new List<YoutubeVideo>();
            while (!youtubeDl.StandardOutput.EndOfStream)
            {
                var jsonOutput = await youtubeDl.StandardOutput.ReadLineAsync();
                videos.Add(JsonConvert.DeserializeObject<YoutubeVideo>(jsonOutput));
            }

            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");
            return videos;
        }

        private static Process StartYoutubeDl(string arguments)
        {
            var youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "Bin\\youtube-dl.exe",
                Arguments = arguments,
            };

            Console.WriteLine($"Starting youtube-dl with arguments: {youtubeDlStartupInfo.Arguments}");
            return Process.Start(youtubeDlStartupInfo);
        }
    }
}
