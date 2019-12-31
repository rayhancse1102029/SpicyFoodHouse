using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SpicyFoodHouse.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String CustomerName { get; set; }
        public byte[] NidOrBith { get; set; }
        public byte[] ProfileImage { get; set; }
        public int Phone { get; set; }
        public String Address { get; set; }
    }
}
