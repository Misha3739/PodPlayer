using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PodPlayer.Models
{
    public class Podcast
    {
        [Key]
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public virtual List<Episode> Episodes { get; set; }
    }


}
