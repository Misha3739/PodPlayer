using System.Threading.Tasks;
using PodPlayer.Models;

namespace PodPlayer.Logic
{
    public interface IParser
    {
        Task<Podcast> GetPodcast(string xml);
    }
}