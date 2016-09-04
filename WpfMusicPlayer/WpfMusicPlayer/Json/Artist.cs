using System.Runtime.Serialization;

namespace WpfMusicPlayer.Json
{
    /// <summary>
    /// Represents Artist json data element.
    /// </summary>
    [DataContract]
    public class Artist
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "albums")]
        public Album[] Albums { get; set; }
    }
}