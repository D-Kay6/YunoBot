using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NYoutubeDL;
using NYoutubeDL.Helpers;

namespace Yuno.Main.Music.YouTube
{
    public class YouTubeDownloadService
    {
        private const string Directory = "Data/Songs"; 
        
        private string DownloadParams => $"-o \"{Directory}/%(id)s.mp3\" --extract-audio --print-json --audio-format mp3 --download-archive \"{Directory}/Archive.txt\" --ignore-errors";

        public async Task<YoutubeVideo> DownloadUrl(string url)
        {
            var youtubeDl = StartYoutubeDl($"{DownloadParams} {url}");

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
