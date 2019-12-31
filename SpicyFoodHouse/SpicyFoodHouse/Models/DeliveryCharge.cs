using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models
{
    public class DeliveryCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [StringLength(200, MinimumLength = 3)]
        [DisplayName("Location Range")]
        public string LocationRange { get; set; }


        [Required]
        [DisplayName("Delivery Charge")]
        public double Charge { get; set; }


        [StringLength(500)]
        public string Description { get; set; }


        public string ManagerSignature { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
