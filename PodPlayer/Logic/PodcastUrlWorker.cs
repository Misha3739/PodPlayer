using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using Foundation;
using PodPlayer.Models;

namespace PodPlayer.Logic
{
    public class PodcastUrlWorker
    {
        private readonly RestClient _restClient;

        public PodcastUrlWorker()
        {
            this._restClient = new RestClient();
        }

        public Podcast GetPodcast(string url)
        {
            var xmlSerializer  = new PodcastSerializer();
            PodcastXml podcastXml = _restClient.Execute<PodcastXml>(url,  xmlSerializer).Data;
            if (podcastXml == null) return null;
            return new Podcast()
            {
                Title = podcastXml.Channel?.Title,
                Url = url
            };
        }

        void HandleNSUrlSessionResponse(NSData data, NSUrlResponse response, NSError error)
        {
        }
    }
}
