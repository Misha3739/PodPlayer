using System.Data.Entity;
using System;
using PodPlayer.Models;
using System.Data.Common;
using System.Configuration;

namespace PodPlayer.Storage
{
    public class PodcastsDBContext : DbContext
    {
        private static readonly string _connectionString;

        static PodcastsDBContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PodplayerConnection"].ConnectionString;
        }

        public PodcastsDBContext() : base(_connectionString)
        {
            
        }

        public DbSet<Podcast> Podcasts { get; set; }


    }
}
