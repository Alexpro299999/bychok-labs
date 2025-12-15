using JewelryStore.Domain.Data;
using JewelryStore.Domain.Models;
using JewelryStore.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.EnsureCreated();

    if (!context.Categories.Any())
    {
        var c1 = new Category { Name = "Серьги" };
        var c2 = new Category { Name = "Кольца" };
        var c3 = new Category { Name = "Ожерелья" };

        context.Categories.AddRange(c1, c2, c3);
        context.SaveChanges();

        context.Products.AddRange(
            new Product { Name = "Золотые серьги", Description = "Изящные серьги из золота 585 пробы", Price = 15000, Material = "Золото", ImageUrl = "/images/earrings1.jpg", Category = c1 },
            new Product { Name = "Серебряные серьги", Description = "Стильные серьги для повседневной носки", Price = 4500, Material = "Серебро", ImageUrl = "/images/earrings2.jpg", Category = c1 },
            new Product { Name = "Золотое кольцо", Description = "Кольцо с драгоценным камнем", Price = 25000, Material = "Золото", ImageUrl = "/images/ring1.jpg", Category = c2 },
            new Product { Name = "Серебряное кольцо", Description = "Элегантное серебряное кольцо", Price = 3000, Material = "Серебро", ImageUrl = "/images/ring2.jpg", Category = c2 },
            new Product { Name = "Ожерелье с жемчугом", Description = "Классическое ожерелье из жемчуга", Price = 8000, Material = "Жемчуг", ImageUrl = "/images/necklace1.jpg", Category = c3 }
        );
        context.SaveChanges();
    }

    if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (await userManager.FindByNameAsync("admin@admin.com") == null)
    {
        var admin = new IdentityUser { UserName = "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();