using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace PodPlayer.Logic.Serializer
{
    public class DataContractPodcastSerializer : ISerializer
    {
        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public T Deserialize<T>(string content)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = Encoding.GetEncoding("ISO-8859-1").GetBytes(content);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var settings = new DataContractSerializerSettings();
                DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }
    }
}
