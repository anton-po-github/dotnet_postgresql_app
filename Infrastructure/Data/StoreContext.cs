using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> product_brands { get; set; }
        public DbSet<ProductType> product_types { get; set; }
    }
}