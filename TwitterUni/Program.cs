using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services;
using TwitterUni.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TwitterDbContextConnection") ?? throw new InvalidOperationException("Connection string 'TwitterDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<TwitterDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TwitterDbContext>();

builder.Services.AddControllersWithViews();

// Adding my services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();