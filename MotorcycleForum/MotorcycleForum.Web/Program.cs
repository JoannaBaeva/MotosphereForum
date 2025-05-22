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
using MotorcycleForum.Services.Admin;
using MotorcycleForum.Services.Marketplace;
using MotorcycleForum.Services.Events;
using MotorcycleForum.Services.Profile;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Load secrets from appsettings.json / appsettings.Development.json
var configuration = builder.Configuration;

// Database
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

string connStr;
if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("postgres://"))
{
    // running in Render (DATABASE_URL is set to something like "postgresql://user:pass@host:port/dbname")
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port == -1 ? 5432 : uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    connStr = connectionStringBuilder.ConnectionString;
}
else
{
    // fallback to local dev
    connStr = configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<MotorcycleForumDbContext>(opts =>
    opts.UseNpgsql(connStr)
);


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

builder.Services.AddScoped<IS3Service>(sp =>
    new S3Service(
        accessKey: configuration["AWS:AccessKey"],
        secretKey: configuration["AWS:SecretKey"],
        bucketName: configuration["AWS:BucketName"],
        regionName: configuration["AWS:Region"]
    ));


// Other Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

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
