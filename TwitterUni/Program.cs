using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Extensions;
using TwitterUni.Services;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.Mapping;

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
});

builder.Services.AddControllersWithViews();

// Adding my services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<ITweetService, TweetService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<ICommentService, CommentService>();

// Adding mapper
var config = new MapperConfiguration(cfg => 
{
    cfg.AddProfile<MappingServiceDataProfile>();
    cfg.AddProfile<MappingViewModelProfile>();
});
builder.Services.AddSingleton(new Mapper(config));

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

app.SeedRolesAndAdminUserAsync().Wait();

app.Run();