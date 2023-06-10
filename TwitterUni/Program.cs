using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data;
using TwitterUni.Data.Entities;
using TwitterUni.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TwitterDbContextConnection") ?? throw new InvalidOperationException("Connection string 'TwitterDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<TwitterDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TwitterDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Auth/Login";
    options.AccessDeniedPath = "/Account/Auth/AccessDenied";
    options.LogoutPath = "/Account/Auth/Logout";
});

builder.Services.AddControllersWithViews();
builder.Services.InjectAppServices();
builder.Services.AddMapping();

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
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});

app.SeedAppData().Wait();

app.Run();