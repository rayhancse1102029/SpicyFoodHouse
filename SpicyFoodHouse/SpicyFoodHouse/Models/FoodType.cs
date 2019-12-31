using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models
{
    public class FoodType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [Required]
        [DisplayName("Food Type")]
        [StringLength(200,MinimumLength = 5, ErrorMessage = "maximum len 200 and min len 5 char")]
        public String TypeName { get; set; }

        [DisplayName("Manager Sig")]
        public String ManagerSignature { get; set; }

        [DisplayName("Entry Date")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get; set; }


        public List<Food> Food { get; set; }
        public List<FoodOrder> FoodOrder { get; set; }

    }
}
