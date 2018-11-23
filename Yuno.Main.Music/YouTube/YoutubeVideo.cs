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

        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "release_date")]
        private string _releaseDate;
        public DateTime ReleaseDate => DateTime.ParseExact(_releaseDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty(PropertyName = "upload_date")]
        private string _uploadDate;
        public DateTime UploadDate => DateTime.ParseExact(_uploadDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

        [JsonProperty(PropertyName = "uploader_id")]
        public string UploaderId { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "view_count")]
        public int? ViewCount { get; set; }

        [JsonProperty(PropertyName = "like_count")]
        public int LikeCount { get; set; }

        [JsonProperty(PropertyName = "dislike_count")]
        public int DislikeCount { get; set; }

        [JsonProperty(PropertyName = "repost_count")]
        public int ReportCount { get; set; }

        [JsonProperty(PropertyName = "average_rating")]
        public double AverageRating { get; set; }

        [JsonProperty(PropertyName = "comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty(PropertyName = "age_limit")]
        public int AgeLimit { get; set; }

        [JsonProperty(PropertyName = "is_live")]
        public bool? IsLive { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        public int? StartTime { get; set; }

        [JsonProperty(PropertyName = "end_time")]
        public int? EndTime { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "format_id")]
        public string FormatId { get; set; }

        [JsonProperty(PropertyName = "format_note")]
        public string FormatNote { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "resolution")]
        public string Resolution { get; set; }

        [JsonProperty(PropertyName = "tbr")]
        public double? Tbr { get; set; }

        [JsonProperty(PropertyName = "abr")]
        public int? Abr { get; set; }

        [JsonProperty(PropertyName = "acodec")]
        public string Acodec { get; set; }

        [JsonProperty(PropertyName = "asr")]
        public int? Asr { get; set; }

        [JsonProperty(PropertyName = "vbr")]
        public int? Vbr { get; set; }

        [JsonProperty(PropertyName = "fps")]
        public int Fps { get; set; }

        [JsonProperty(PropertyName = "vcodec")]
        public string Vcodec { get; set; }

        [JsonProperty(PropertyName = "container")]
        public string Container { get; set; }

        [JsonProperty(PropertyName = "filesize")]
        public int FileSize { get; set; }

        [JsonProperty(PropertyName = "filesize_approx")]
        public int FileSizeApprox { get; set; }

        [JsonProperty(PropertyName = "protocol")]
        public string Protocol { get; set; }

        [JsonProperty(PropertyName = "extractor")]
        public string Extractor { get; set; }

        [JsonProperty(PropertyName = "extractor_key")]
        public string ExtractorKey { get; set; }

        [JsonProperty(PropertyName = "epoch")]
        public int Epoch { get; set; }

        [JsonProperty(PropertyName = "autonumber")]
        public int AutoNumber { get; set; }

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
