namespace Logic.Models.Music.Search
{
    public class Playlist
    {
        public Playlist(string name, int selectedTrack)
        {
            Name = name;
            SelectedTrack = selectedTrack;
        }

        public string Name { get; }
        public int SelectedTrack { get; }
    }
}