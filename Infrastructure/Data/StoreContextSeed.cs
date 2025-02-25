using System.Reflection;
using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {

            // var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.products.Any())
            {
                //  var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                var productsData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.products.AddRange(products);
            }

            if (!context.product_brands.Any())
            {
                //  var brandsData = File.ReadAllText(path + @"/Data/SeedData/brands.json");
                var brandsData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.product_brands.AddRange(brands);
            }

            if (!context.product_types.Any())
            {
                // var typesData = File.ReadAllText(path + @"/Data/SeedData/types.json");
                var typesData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.product_types.AddRange(types);
            }

            if (!context.delivery_method.Any())
            {
                var deliveryData = File.ReadAllText("../../../../DOT_NET/dotnet_postgresql/Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                context.delivery_method.AddRange(methods);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}