using KutuphaneOtomasyon;
using KutuphaneOtomasyon.Models;
using KutuphaneOtomasyon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = "UserCookies";
}
)
    .AddCookie("AdminCookies", options =>
    {
        options.LoginPath = "/Admin/Login";       // Admin login sayfasýna yönlendirilecek
        options.AccessDeniedPath = "/AccessDenied"; // Yetkisiz eriþim sayfasý
    })
    .AddCookie("UserCookies", options =>
    {
        options.LoginPath = "/Login/Index";       // Kullanýcý login sayfasýna yönlendirilecek
        options.AccessDeniedPath = "/AccessDenied"; // Yetkisiz eriþim sayfasý
    });




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));

    options.AddPolicy("User", policy => policy.RequireRole("User"));
});


builder.Services.AddAuthorization();  
builder.Logging.AddConsole();
builder.Services.AddScoped<IsbnService>();
builder.Services.AddScoped<Cezaservice>();
builder.Services.AddScoped<CezaArkaPlanService>();
builder.Services.AddHostedService<CezaArkaPlanService>();
builder.Services.AddTransient<IEmailService, EmailService>();

 


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

app.UseSession();  
app.UseAuthentication();  
app.UseAuthorization();   


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 

app.MapControllerRoute(
    name: "Login",
    pattern: "Login",
    defaults: new { controller = "Login", action = "Index" }


);
app.MapControllerRoute(
    name: "Login",
    pattern: "AdminGiris",
    defaults: new { controller = "Admin", action = "Login" }


);
app.Run();
