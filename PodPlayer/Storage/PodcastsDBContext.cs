using System.Data.Entity;
using System;
using PodPlayer.Models;
using System.Data.Common;
using System.Configuration;

namespace PodPlayer.Storage
{
    public class PodcastsDBContext : DbContext
    {
        public PodcastsDBContext() : base("name = PodplayerConnection")
        {
            
        }

        public DbSet<Podcast> Podcasts { get; set; }

        public DbSet<Episode> Episodes { get; set; }
    }
}
