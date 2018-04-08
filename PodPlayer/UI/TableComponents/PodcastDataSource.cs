using System;
using System.Collections;
using System.Collections.Generic;
using AppKit;
using System.Linq;

using PodPlayer.Models;
using PodPlayer.Storage;
using System.Data.SqlClient;

namespace PodPlayer.UI.TableComponents
{
    public class PodcastDataSource : NSTableViewDataSource, IDisposable
    {
        private IList<Podcast> _podcasts;

        public IList<Podcast> Podcasts => _podcasts;

        private PodcastsDBContext _context;

        public PodcastDataSource()
        {
            _podcasts = new List<Podcast>();

            _context = new PodcastsDBContext();
        }

        public void GetPodcasts()
        {
            try{
                _podcasts = _context.Podcasts.ToList();
            }
            catch(SqlException e)
            {
                throw;
            }
            catch(Exception e)
            {
                throw;
            }
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

		protected override void Dispose(bool disposing)
		{
            _context?.Dispose();
            base.Dispose(disposing);
		}

	}
}
