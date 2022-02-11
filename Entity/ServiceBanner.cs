using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Entity
{
    [Table("ServiceBanner")]
    public class ServiceBanner
    {
        [Key]
        public int ServiceBannerId { get; set; }
        public int ServiceId { get; set; }
        public string BannerUrl { get; set; }
        public int IsDefault { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}