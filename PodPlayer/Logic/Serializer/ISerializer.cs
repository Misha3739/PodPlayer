using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
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
}