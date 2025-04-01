using Microsoft.EntityFrameworkCore;
using Server.Models.Account;
using Server.Models.Device;
using Server.Models.Product;
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
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountModel>()
                    .HasOne(a => a.User)
                    .WithMany() 
                    .HasForeignKey("AccountId") 
                    .IsRequired();

            modelBuilder.Entity<DeviceModel>()
                    .HasOne(d => d.Room)
                    .WithMany(r => r.Devices)
                    .HasForeignKey(d => d.RoomId)
                    .IsRequired();
            modelBuilder.Entity<ProductModel>();
        }
    }
}
