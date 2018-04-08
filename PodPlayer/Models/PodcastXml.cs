using System;
using System.Xml.Serialization;

namespace PodPlayer.Models
{
    [Serializable]
    [XmlRoot(ElementName = "rss")]
    public class PodcastXml
    {
        public PodcastXml()
        {
        }
        
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
    }

    [Serializable]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
    }
}
