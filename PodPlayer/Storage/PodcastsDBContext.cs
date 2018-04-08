using System.Data.Entity;
using System;
using PodPlayer.Models;
using System.Data.Common;

namespace PodPlayer.Storage
{
    public class PodcastsDBContext : DbContext
    {
        private static readonly string _connectionString = @"Server=localhost;Database=PodPlayer;User Id=sa;Password=Fergana12;";

        public PodcastsDBContext() : base(_connectionString)
        {
            
        }

        public DbSet<Podcast> Podcasts { get; set; }


    }
}
