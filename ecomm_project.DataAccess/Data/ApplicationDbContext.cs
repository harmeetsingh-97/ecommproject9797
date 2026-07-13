using ecomm_project.models;
using ecomm_project.models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecomm_project.DataAccess.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<category> Categories { get; set; }
        public DbSet<covertype> Covertypes { get; set; }
      public DbSet<product> Products { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }


        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> orderHeader { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }


    }
}
