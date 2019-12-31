using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpicyFoodHouse.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        //  Custom property 

        [Required]
        [DisplayName("Customer Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "maximum len 30 and min len 3 char")]
        public String CustomerName { get; set; }


        [Required]
        [DisplayName("NID/BIRTH Certificate")]
        public byte[] NidOrBith { get; set; }

        [Required]
        [DisplayName("Contact Number")]
        [Phone]
        public int Phone { get; set; }


        [Required]
        [DisplayName("Delivery Address")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "maximum len 200 and min len 20 char")]
        public String Address { get; set; }

        [Required]
        [DisplayName("Profile Image")]
        public byte[] ProfileImage { get; set; }



    }
}
