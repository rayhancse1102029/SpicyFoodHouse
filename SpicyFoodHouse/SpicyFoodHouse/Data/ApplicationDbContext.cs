using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<SpicyFoodHouse.Models.Food> Food { get; set; }

        public DbSet<SpicyFoodHouse.Models.FoodOrder> FoodOrder { get; set; }

        public DbSet<SpicyFoodHouse.Models.FoodQuarter> FoodQuarter { get; set; }

        public DbSet<SpicyFoodHouse.Models.FoodType> FoodType { get; set; }

        public DbSet<SpicyFoodHouse.Models.PaymentMethod> PaymentMethod { get; set; }

        public DbSet<SpicyFoodHouse.Models.Comment> Comment { get; set; }

        public DbSet<SpicyFoodHouse.Models.DeliveryCharge> DeliveryCharge { get; set; }

        public DbSet<SpicyFoodHouse.Models.AvailableInStock> AvailableInStock { get; set; }
    }
}
