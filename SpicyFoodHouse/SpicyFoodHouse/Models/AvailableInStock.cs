using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models
{
    public class AvailableInStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [StringLength(200, MinimumLength = 3)]
        [DisplayName("Food Name")]
        public string FoodName { get; set; }


        [Required]
        [DisplayName("Delivery Charge")]
        public int AvailableFood { get; set; }


        [StringLength(500)]
        public string Description { get; set; }


        public string ManagerSignature { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
