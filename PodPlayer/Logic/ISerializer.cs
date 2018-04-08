using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PodPlayer.Logic
{
	public interface ISerializer
	{
		T Deserialize<T>(string content);
	}

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

    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}