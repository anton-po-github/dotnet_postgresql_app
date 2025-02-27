using API.Errors;
using API.Helpers;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseSqlite(config.GetConnectionString("IdentityConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddSwaggerDoc();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                                  {
                                      var errors = actionContext.ModelState
                                      .Where(e => e.Value.Errors.Count > 0)
                                      .SelectMany(x => x.Value.Errors)
                                      .Select(x => x.ErrorMessage).ToArray();

                                      var errorResponse = new ApiValidationErrorResponse
                                      {
                                          Errors = errors
                                      };

                                      return new BadRequestObjectResult(errorResponse);
                                  };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowAnyOrigin();
                    policy.WithOrigins("https://r.stripe.com").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}