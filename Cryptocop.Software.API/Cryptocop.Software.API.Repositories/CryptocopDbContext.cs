using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories.Entities;

namespace Cryptocop.Software.API.Repositories
{
    public class CryptocopDbContext : DbContext
    {
        public CryptocopDbContext(DbContextOptions<CryptocopDbContext> options) : base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCartItem>()
                .HasKey(x => new {x.Id, x.ShoppingCartId});
            modelBuilder.Entity<PaymentCard>()
                .HasKey(x => new {x.Id, x.UserId});
            modelBuilder.Entity<OrderItem>()
                .HasKey(x => new {x.Id, x.OrderId});
            modelBuilder.Entity<Address>()
                .HasKey(x => new {x.Id, x.UserId});

        }
        
        public DbSet<Address> Address { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<PaymentCard> PaymentCards { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<JwtToken> JwtTokens { get; set; } = null!;
     }
}