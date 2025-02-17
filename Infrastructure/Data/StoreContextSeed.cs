using System.Reflection;
using System.Text.Json;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.Products.Any())
            {
                //  var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                var productsData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }

            if (!context.ProductBrands.Any())
            {
                //  var brandsData = File.ReadAllText(path + @"/Data/SeedData/brands.json");
                var brandsData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                // var typesData = File.ReadAllText(path + @"/Data/SeedData/types.json");
                var typesData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}