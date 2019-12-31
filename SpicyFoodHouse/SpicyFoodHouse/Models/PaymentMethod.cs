using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models
{
    public class PaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }

        [Required]
        [DisplayName("Payment method name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "maximum len 100 and min len 3 char")]
        public String PaymentMethodName { get; set; }


        [DisplayName("Manager Sig")]
        public String ManagerSignature { get; set; }

        [DisplayName("Entry Date")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get; set; }

        public List<FoodOrder> FoodOrder { get; set; }
    }
}
