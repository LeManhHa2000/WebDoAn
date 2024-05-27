using Microsoft.EntityFrameworkCore;
using WebDoAn.Models;

namespace WebDoAn.dbs
{
    public class DoAnDbContext : DbContext
    {
        public DoAnDbContext(DbContextOptions<DoAnDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region DoAn
        public DbSet<User> user { get; set; }
        public DbSet<Category> categorie { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<ProductImg> productImg { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderDetail> orderDetail { get; set; }
        //public DbSet<Shipper> shipper { get; set; }
        public DbSet<Blog> blog { get; set; }
        public DbSet<Cart> cart { get; set; }
        //public DbSet<Comment> comment { get; set; }


        #endregion
    }
}
