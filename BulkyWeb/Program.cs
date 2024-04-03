using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;
using Stripe;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add EF to the project, and adding dbcontext to the container
// Grabs connection string "DefaultConnection" from ApplicationDbContext.
builder.Services.AddDbContext<ApplicationDbContext>(options=> 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// If the property names in StripeSettings class match those of Stripe in appsettings.json they will be populated.
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// IMPT this had to procede AddIdentity
// ConfigureApplicationCookie overrides default application paths for Identity since we moved it to Identity.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Session is not added by default, you have to configure it yourself if you want it. So the below two items are needed.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// You have to dependency inject the DbInitializer to use it.
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// So app works with the new Identity Razor Pages
builder.Services.AddRazorPages();

// You need to register the service in the dependency injection container, lifetime scoped, one request uses same service.
// Adding the CategoryRepository in the dependency injection container
// Before UnitOfWork added.
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Since we are now using IdentityUser with IdentityRole, we are no longer using the fake implentation and therefore
//need to implement and inject emailsender.
builder.Services.AddScoped<IEmailSender, EmailSender>();

//  GEF Added Google logon
var configuration = builder.Configuration;

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
// Is user name and or passwor valid?
app.UseAuthentication();
// If user is Authenticated, Authorize the user based on their role.
app.UseAuthorization();
app.UseSession(); // GEF note added this in order to use session in Application
// Invoking SeedDatabase in our pipeline. Invoked everytime the application is restarted.
SeedDatabase();
// Also needed to support Razor Pages.
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}