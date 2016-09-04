using System.Runtime.Serialization;

namespace WpfMusicPlayer.Json
{
    /// <summary>
    /// Represents Song json data element.
    /// </summary>
    [DataContract]
    public class Song
    {
        [DataMember(Name = "length")]
        public string Length { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "favorite")]
        public bool Favorite { get; set; }
    }
}