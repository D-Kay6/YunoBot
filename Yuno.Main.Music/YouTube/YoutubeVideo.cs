using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace Yuno.Main.Music.YouTube
{
    public class YoutubeVideo : IPlayable
    {
        public string Uri => Url;
        public IUser Requester { get; set; }
        public IMessageChannel TextChannel { get; set; }
        public string DurationString => TimeSpan.FromSeconds(Duration).ToString();
        public int Volume { get; set; } = 25;
        public int Speed { get; set; } = 48;
        public void OnPostPlay()
        {
        }
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "ext")]
        public string Extention { get; set; }

        [JsonProperty(PropertyName = "alt_title")]
        public string AltTitle { get; set; }

        [JsonProperty(PropertyName = "display_id")]
        public string DisplayId { get; set; }

        [JsonProperty(PropertyName = "uploader")]
        public string Uploader { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "uploader_id")]
        public string UploaderId { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "playlist")]
        public string Playlist { get; set; }

        [JsonProperty(PropertyName = "playlist_index")]
        public int? PlaylistIndex { get; set; }

        [JsonProperty(PropertyName = "playlist_id")]
        public string PlaylistId { get; set; }

        [JsonProperty(PropertyName = "playlist_title")]
        public string PlaylistTitle { get; set; }

        [JsonProperty(PropertyName = "playlist_uploader")]
        public string PlaylistUploader { get; set; }

        [JsonProperty(PropertyName = "playlist_uploader_id")]
        public string PlaylistUploaderId { get; set; }
    }
}
