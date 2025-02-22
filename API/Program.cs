using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddApplicationServices(builder.Configuration);
services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwaggerDoc();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

/* app.UseCors(builder =>
builder.WithOrigins("https://localhost:4200")
.AllowAnyHeader()
.AllowCredentials()
.AllowAnyMethod()); */

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<StoreContext>();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var identityContext = serviceProvider.GetRequiredService<AppIdentityDbContext>();
var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);

    await identityContext.Database.MigrateAsync();
    //await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
