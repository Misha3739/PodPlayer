using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using Foundation;
using PodPlayer.Logic.Serializer;
using PodPlayer.Models;
using System.Xml.Linq;
using System.Xml;
using Terradue.ServiceModel.Syndication;
using System.Collections.Generic;

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
            Podcast podcast = new Podcast()
            {
                Title = podcastXml.Channel?.Title,
                ImageUrl = podcastXml.Channel?.Image?.Url,
                Url = url
            };

            return podcast;
        }

        public Podcast GetPodcast2(string url)
        {
            try
            {
                XNamespace ns = "http://purl.org/dc/elements/1.1/";
                XDocument feedXML = XDocument.Load(url);
                Podcast podcast = new Podcast();
                podcast.Title = feedXML.Descendants("title").FirstOrDefault()?.Value;
                podcast.ImageUrl = feedXML.Descendants($"{ns}image").FirstOrDefault()?.Attribute("href")?.Value;
                podcast.Url = url;
                var episodes = from feed in feedXML.Descendants("item")
                               select new Episode
                               {
                                   Title = feed.Element("title").Value,
                                   AudioUrl = feed.Element("media:group").Attribute("url")?.Value,
                                   Description = feed.Element("description").Value
                               };
                podcast.Episodes = episodes.ToList();
                return podcast;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public Podcast GetPodcast3(string url){
            Podcast podcast = new Podcast();
            SyndicationFeed feed = null;
            using(XmlReader reader = XmlReader.Create(url))
            {
                SyndicationFeed.Load(reader);
            }

            podcast.Title = feed.Title.Text;
            podcast.ImageUrl = feed.ImageUrl.ToString();
            podcast.Episodes = new List<Episode>();
            foreach (SyndicationItem item in feed.Items)
            {
                Episode episode = new Episode();
                episode.Title = item.Title.Text;
                episode.AudioUrl = item.BaseUri.ToString();
                episode.PublishDate = item.PublishDate.DateTime;
            }
            return podcast;
        }

    }
}
