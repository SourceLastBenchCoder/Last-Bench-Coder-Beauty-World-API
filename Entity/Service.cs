using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Entity
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public string ServiceCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Store Store { get; set; }
        public Category Category { get; set; }
        public ICollection<ServiceBanner> ServiceBanner { get; set; }
    }
}