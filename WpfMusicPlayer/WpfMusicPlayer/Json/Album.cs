using System.Runtime.Serialization;

namespace WpfMusicPlayer.Json
{
    /// <summary>
    /// Represents Album json data element.
    /// </summary>
    [DataContract]
    public class Album
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "image")]
        public string Image { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "date")]
        public string Date { get; set; }
        [DataMember(Name = "songs")]
        public Song[] Songs { get; set; }

    }
}