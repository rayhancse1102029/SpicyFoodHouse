using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SpicyFoodHouse.Models
{
    public class FoodOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        
        public string CustomerEmail { get; set; }

        // For Single Order Default


        [Required]
        [DisplayName("Food Item Type")]
        public int TypeId { get; set; }

        [Required]
        [DisplayName("Food Name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "maximum len 200 and min len 3 char")]
        public String FoodName { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        [DisplayName("Number of Food")]
        public int NumberOfFood { get; set; }

        //[Required]
        [DisplayName("Quarter")]
        public int QuarterId { get; set; }

        [Required]
        [DisplayName("Total Price")]
        public float TotalPrice { get; set; }

        
        // For Multiple Order Two

        //[Required]
        [DisplayName("Food Item Type")]
        public int TypeId2 { get; set; }

        //[Required]
        [DisplayName("Food Name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "maximum len 200 and min len 3 char")]
        public String FoodName2 { get; set; }

        //[Required]
        public float Price2 { get; set; }

        //[Required]
        [DisplayName("Number of Food")]
        public int NumberOfFood2 { get; set; }

        //[Required]
        [DisplayName("Quarter")]
        public int QuarterId2 { get; set; }

        //[Required]
        [DisplayName("Total Price of 2nd Food")]
        public float TotalPrice2 { get; set; }


        // For Multiple Order Three

        //[Required]
        [DisplayName("Food Item Type")]
        public int TypeId3 { get; set; }

        //[Required]
        [DisplayName("Food Name")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "maximum len 200 and min len 3 char")]
        public String FoodName3 { get; set; }

        //[Required]
        public float Price3 { get; set; }

        //[Required]
        [DisplayName("Number of Food")]
        public int NumberOfFood3 { get; set; }

        //[Required]
        [DisplayName("Quarter")]
        public int QuarterId3 { get; set; }

        //[Required]
        [DisplayName("Total Price of 3rd Food")]
        public float TotalPrice3 { get; set; }


        // For Multiple Order

        //[Required]
        [DisplayName("All Total Price")]
        public float SubTotalPrice { get; set; }

  
        [Required]
        [DisplayName("Payment Method")]
        public int PaymentMethodId { get; set; }

        //[Required]
        [DisplayName("Enter the last 5 digits of Bkash number")]
        public int LastFiveDigit { get; set; }

        public bool IsPaid { get; set; } = false;

        public bool IsSeen { get; set; } = false;

        public bool IsAccepted { get; set; } = false;

        public bool IsRejected { get; set; } = false;


        [DisplayName("Order Time")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Last Updated Time")]
        public DateTime LastUpdatedDate { get; set; }


        public FoodType FoodType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public FoodQuarter FoodQuarter { get; set; }

    }
}
