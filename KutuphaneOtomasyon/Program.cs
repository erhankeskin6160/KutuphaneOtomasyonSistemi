using KutuphaneOtomasyon.Models;
using KutuphaneOtomasyon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddAuthentication("Cookies") // Varsayýlan kimlik doðrulama þemasý
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login";          // Giriþ sayfasý
        options.AccessDeniedPath = "/AccessDenied"; // Yetkisiz eriþim
    });

builder.Services.AddAuthorization(); // Authorization middleware

builder.Services.AddScoped<IsbnService>();



builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Oturum middleware
app.UseAuthentication(); // Authentication middleware
app.UseAuthorization();  // Authorization middleware



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

  app.Run();
