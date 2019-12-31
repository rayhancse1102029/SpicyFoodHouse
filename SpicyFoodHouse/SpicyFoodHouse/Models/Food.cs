using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models
{
    public class Food 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodId { get; set; }

        [Required]
        [DisplayName("Food Name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "maximum len 200 and min len 3 char")]
        public String FoodName { get; set; }

        [Required]
        [DisplayName("Food Item Type")]
        public int TypeId { get; set; }


        [Required]
        [DisplayName("Food Image")]
        public byte[] Image { get; set; }


        [Required]
        [DisplayName("Description")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "maximum len 200 and min len 20 char")]
       public String Description{ get; set; }

       [Required]
       public float Price { get; set; }

       [Required]
       [DisplayName("Available in Stock")]
       public string AvailableInStock { get; set; }

       [Required]
       [DisplayName("Discount in % ")]
       public float Discount { get; set; }

       [Required]
       public bool IsTranding { get; set; }

       [Required]
       public bool IsPopular { get; set; }

       [Required]
       public bool IsDiscounted { get; set; }

        [DisplayName("Manager Sig")]
       public String ManagerSignature { get; set; }

       [DisplayName("Entry Date")]
       public DateTime EntryDate { get; set; }

       [DisplayName("Last Updated Date")]
       public DateTime LastUpdatedDate { get; set; }


        public FoodType FoodType { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
