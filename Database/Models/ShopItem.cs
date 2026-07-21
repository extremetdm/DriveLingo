using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class ShopItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        public int Cost { get; set; }

        public virtual ICollection<ShopRedemption> Redemptions { get; set; } = new HashSet<ShopRedemption>();
    }
}