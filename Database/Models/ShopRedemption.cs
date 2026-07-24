using DriveLingo.Database.Models.Traits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class ShopRedemption: Timestamps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual ShopItem Item { get; set; }
    }
}