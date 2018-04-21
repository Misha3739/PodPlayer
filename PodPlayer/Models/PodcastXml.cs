using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PodPlayer.Models
{
    [Serializable]
    [XmlRoot(ElementName = "rss")]
    public class PodcastXml
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
    }

    [Serializable]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "itunes:author", Type = typeof(string))]
        public string Author { get; set; }

        [XmlElement(ElementName = "itunes:image")]
        public Image Image { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
    }

    [Serializable]
    public class Image {

        [XmlAttribute("href")]
        public string Url { get; set; }
    }
}
