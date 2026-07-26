using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class ShopItem
    {
        public enum ItemType
        {
            Border = 0,
            Icon = 1,
            Badge = 2
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Icon { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public ItemType Type { get; set; }

        public string ColorHex { get; set; }

        public virtual ICollection<ShopRedemption> Redemptions { get; set; } = new HashSet<ShopRedemption>();
    }
}