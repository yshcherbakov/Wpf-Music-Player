using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfMusicPlayer.Helpers
{
    public class Translator
    {
        public List<Model.Artist> Translate(Json.Music music)
        {
            var result = new List<Model.Artist>();

            if (music == null || music.Artists == null || !music.Artists.Any()) 
                return result;

            foreach (var artistJson in music.Artists.OrderBy(i => i.Name))
            {
                var artist = new Model.Artist { Name = artistJson.Name };

                foreach (var albumJason in artistJson.Albums.OrderBy(i => i.Date))
                {
                    var album = new Model.Album
                    {
                        Title = albumJason.Title,
                        Description = albumJason.Description,
                        Image = albumJason.Image,
                        Date = DateTime.Parse(albumJason.Date),
                    };

                    foreach (var songJason in albumJason.Songs)
                    {
                        var song = new Model.Song
                        {
                            Title = songJason.Title,
                            Length = songJason.Length,
                            Favorite = songJason.Favorite,
                        };

                        album.Songs.Add(song);
                    }

                    artist.Albums.Add(album);
                }

                result.Add(artist);
            }

            return result;
        }
    }
}
