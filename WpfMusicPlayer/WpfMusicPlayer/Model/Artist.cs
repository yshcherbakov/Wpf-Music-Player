using System.Collections.Generic;

namespace WpfMusicPlayer.Model
{
    /// <summary>
    /// Represents Artist entity.
    /// </summary>
    public class Artist
    {
        public string Name { get; set; }
        public List<Album> Albums { get; set; }

        public Artist() 
        {
            Albums = new List<Album>();
        }
    }
}