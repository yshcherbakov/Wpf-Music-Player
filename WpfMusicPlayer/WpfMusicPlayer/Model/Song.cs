namespace WpfMusicPlayer.Model
{
    /// <summary>
    /// Represents Song entity.
    /// </summary>
    public class Song
    {
        public string Length { get; set; }
        public string Title { get; set; }
        public bool Favorite { get; set; }
        public string Image { get { return (Favorite) ? @"~\..\Images\favorite.png" : string.Empty; } }
    }
}