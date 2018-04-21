using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace PodPlayer.Logic.Serializer
{
    public class PodcastSerializer : ISerializer
    {

        public T Deserialize<T>(string content)
        {
            Debug.WriteLine(content);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var xmlReader = new XmlTextReader(new StringReader(content)))
            {
                object deserialized = serializer.Deserialize(xmlReader);
                return (T)deserialized;

            }

        }
    }
}
