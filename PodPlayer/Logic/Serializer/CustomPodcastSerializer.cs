using System;
using System.Reflection;
using System.Xml.Serialization;

namespace PodPlayer.Logic.Serializer
{
    public class CustomPodcastSerializer : ISerializer
    {
        public T Deserialize<T>(string content)
        {
            try
            {
                XmlRootAttribute rootAttribute = typeof(T).GetCustomAttribute(typeof(XmlRootAttribute)) as XmlRootAttribute;
                if (rootAttribute == null) throw new Exception($"Type {typeof(T).Name} should be marked with XmlRoot attribute!");

                T result = Activator.CreateInstance<T>();



                return result;
            }
            catch(Exception e)
            {
                throw new FormatException($"Serialization failed on error: {e.Message}", e);
            }
        }

        private T Deserialize<T>(string content, string elementName)
        {
            T result = Activator.CreateInstance<T>();


            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                var xmlElementAttribute = propertyInfo.GetCustomAttribute(typeof(XmlElementAttribute), true);
                if (xmlElementAttribute != null)
                {
                    string name = (xmlElementAttribute as XmlElementAttribute).ElementName;

                }

            }


            return result;
        }
    }
}
