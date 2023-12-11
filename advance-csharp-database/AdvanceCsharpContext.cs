using advance_csharp.database.Models;
using Microsoft.EntityFrameworkCore;

namespace advance_csharp.database
{
    public class AdvanceCsharpContext : DbContext
    {

        /// <summary>
        /// Connectionstring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        public AdvanceCsharpContext(DbContextOptions<AdvanceCsharpContext> options) : base(options)
        {
        }

        

        /// <summary>
        /// AppVersions
        /// </summary>
        public DbSet<AppVersion>? AppVersions { get; set; }

        /// <summary>
        /// Products
        /// </summary>
        public DbSet<Product>? Products { get; set; }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<User>? Users { get; set; }


        /// <summary>
        /// Cart
        /// </summary>
        public DbSet<Cart> Carts { get; set; } = null!;

        /// <summary>
        /// Cart detail
        /// </summary>
        public DbSet<CartDetail>? CartDetails { get; set; }

        /// <summary>
        /// Orders
        /// </summary>
        public DbSet<Order>? Orders { get; set; }

        /// <summary>
        /// OrderDetails
        /// </summary>
        public DbSet<OrderDetail>? OrderDetails { get; set; }

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> SaveChangesAsync(string email)
        {
            throw new NotImplementedException();
        }


    }
}