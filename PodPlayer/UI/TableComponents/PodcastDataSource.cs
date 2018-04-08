using System;
using System.Collections;
using System.Collections.Generic;
using AppKit;
using System.Linq;

using PodPlayer.Models;

namespace PodPlayer.UI.TableComponents
{
    public class PodcastDataSource : NSTableViewDataSource
    {
        private IList<Podcast> _podcasts;

        public IList<Podcast> Podcasts => _podcasts;

        public PodcastDataSource()
        {
            _podcasts = new List<Podcast>();
        }

		public override nint GetRowCount(NSTableView tableView)
		{
            return _podcasts?.Count ?? 0;
		}

        public void AddPodcast(Podcast podcast)
        {
            if (this.Podcasts.Any(p => p.Url == podcast.Url))
                return;
            this.Podcasts.Add(podcast);
        }

	}
}
