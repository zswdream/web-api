using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SwitDish.DataModel.Models
{
    public partial class SwitDishDbContext : DbContext
    {
        public SwitDishDbContext()
        {
        }

        public SwitDishDbContext(DbContextOptions<SwitDishDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-K2MO1ED\\SQLEXPRESS;Database=SwitDishDb;Integrated Security=True");
            }
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AppNLog> AppNLogs { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerBooking> CustomerBookings { get; set; }
        public DbSet<CustomerDeliveryAddress> CustomerDeliveryAddresses { get; set; }
        public DbSet<CustomerFavouriteVendor> CustomerFavouriteVendors { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderProduct> CustomerOrderProducts { get; set; }
        public DbSet<CustomerSecurityQuestion> CustomerSecurityQuestions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorCuisine> VendorCuisines { get; set; }
        public DbSet<VendorDeliveryTime> VendorDeliveryTimes { get; set; }
        public DbSet<VendorGalleryImage> VendorGalleryImages { get; set; }
        public DbSet<VendorOffer> VendorOffers { get; set; }
        public DbSet<VendorServiceCategory> VendorServiceCategories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<VendorFeature> VendorFeatures { get; set; }
        public DbSet<CustomerOrderFeedback> CustomerOrderFeedbacks { get; set; }
        public DbSet<VendorFeedbackReaction> VendorFeedbackReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<CustomerSecurityQuestion>()
                .HasKey(d=> new { d.CustomerId, d.SecurityQuestionId });

            modelBuilder.Entity<CustomerFavouriteVendor>()
                .HasKey(d => new { d.CustomerId, d.VendorId });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
