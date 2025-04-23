using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Services;
using MotorcycleForum.Services.Forum;
using Microsoft.Extensions.Configuration;
using MotorcycleForum.Services.Marketplace;
using MotorcycleForum.Services.Events;

var builder = WebApplication.CreateBuilder(args);

// Load secrets from appsettings.json / appsettings.Development.json
var configuration = builder.Configuration;

// Database
var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MotorcycleForumDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<MotorcycleForumDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddSingleton<S3Service>(sp =>
{
    var awsSection = configuration.GetSection("AWS");

    var accessKey = awsSection["AccessKey"];
    var secretKey = awsSection["SecretKey"];
    var bucketName = awsSection["BucketName"];
    var region = awsSection["Region"];

    return new S3Service(accessKey, secretKey, bucketName, region);
});

// Other Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
builder.Services.AddScoped<IEventsService, EventsService>();
//builder.Services.AddScoped<IAdminService, AdminService>();
//builder.Services.AddScoped<IProfileService, ProfileService>();

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
    app.UseMigrationsEndPoint();
else
    app.UseHsts();

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
