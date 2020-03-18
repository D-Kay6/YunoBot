namespace Logic.Models.Music.Search
{
    using System.Collections.Generic;
    using Track;

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