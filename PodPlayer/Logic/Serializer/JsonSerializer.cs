using System;
using Newtonsoft.Json;

namespace PodPlayer.Logic.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
