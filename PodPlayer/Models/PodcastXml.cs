using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PodPlayer.Models
{
    [Serializable]
    [XmlRoot(ElementName = "rss")]
    [DataContract(Name = "rss")]
    public class PodcastXml
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
    }

    [Serializable]
    [DataContract(Name = "channel")]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "itunes_x003A_author")]
        [DataMember(Name = "itunes_x003A_author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "itunes_x003A_image")]
        [DataMember(Name = "itunes_x003A_image")]
        public Image Image { get; set; }

        [XmlElement(ElementName = "link")]
        [DataMember(Name = "link")]
        public string Link { get; set; }
    }

    [Serializable]
    public class Image {

        [XmlAttribute("href")]
        public string Url { get; set; }
    }
}
