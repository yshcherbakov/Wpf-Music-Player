using System;
using System.Collections.Generic;

namespace WpfMusicPlayer.Model
{
    /// <summary>
    /// Represents Album entity.
    /// </summary>
    public class Album
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public List<Song> Songs { get; set; }

        public Album()
        {
            Songs = new List<Song>();
        }
    }
}