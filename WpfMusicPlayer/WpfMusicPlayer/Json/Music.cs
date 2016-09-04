using System.Runtime.Serialization;

namespace WpfMusicPlayer.Json
{
    /// <summary>
    /// Represents the music data root json data element.
    /// </summary>
    [DataContract]
    public class Music
    {
        [DataMember(Name = "artists")]
        public Artist[] Artists { get; set; }
    }
}
