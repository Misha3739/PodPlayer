using System;
using System.ComponentModel.DataAnnotations;

namespace PodPlayer.Models
{
    public class Podcast
    {
        [Key]
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}
