using Microsoft.EntityFrameworkCore;
using Server.Models.Account;
using Server.Models.Device;
using Server.Models.Order;
using Server.Models.Product;
using Server.Models.Review;
using Server.Models.Room;
using Server.Models.User;

namespace Server.Data
{
    public class DatabaseContext : DbContext
    {
        // Constructor for DbContext
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // DbSet for Account
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ActivationCodeModel> ActivationCodes { get; set; }

        // DbSet for User
        public DbSet<UserModel> Users { get; set; }

        // Dbset for Facility

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountModel>()
                    .HasOne(a => a.User)
                    .WithMany()
                    .HasForeignKey("AccountId")
                    .IsRequired();

            modelBuilder.Entity<ProductModel>();
            modelBuilder.Entity<ReviewModel>();
            modelBuilder.Entity<OrderModel>();
            modelBuilder.Entity<OrderItemModel>();

        }
    }
}
