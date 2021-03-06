﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PodPlayer.Models
{
    public class Episode
    {
        [Key]
        public long? Id { get; set; }

        public DateTime? PublishDate { get; set; }

        public string AudioUrl { get; set; }

        public string Description { get; set; }

        public virtual Podcast Podcast { get; set; }
    }
}
