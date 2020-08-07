using Logic.Models.Music.Track;
using System.Collections.Generic;

namespace Logic.Models.Music.Search
{
    public class SearchResult
    {
        public SearchResult(IReadOnlyList<ITrack> tracks, ResultStatus resultStatus, Playlist playlist = null,
            string exception = null)
        {
            Tracks = tracks;
            ResultStatus = resultStatus;
            Playlist = playlist;
            Exception = exception;
        }

        public IReadOnlyList<ITrack> Tracks { get; }
        public ResultStatus ResultStatus { get; }
        public Playlist Playlist { get; }
        public string Exception { get; }
    }
}