using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestOgSikkerhed.Components.Account;
using TestOgSikkerhed.Components;
using TestOgSikkerhed.Data;
using TestOgSikkerhed.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for HTTPS with a certificate
string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string certPath = Path.Combine(userFolder, ".aspnet", "https", "DolaCerts.pfx"); // Path to your DolaCert.pfx file
string certPassword = "Passw0rd"; // Password for the certificate
//to get docker to work comment out this and the kerstel https stuff in appsettings
builder.WebHost.UseKestrel((context, serverOptions) =>
{
    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.ServerCertificate = new X509Certificate2(certPath, certPassword); // Load the certificate
    });
});

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Require email confirmation for login
    options.SignIn.RequireConfirmedAccount = true;
});

// Declare connectionString variable
string connectionString;

// Determine which database to use
if (OperatingSystem.IsLinux())
{
    connectionString = builder.Configuration.GetConnectionString("MockDBConnection")
        ?? throw new InvalidOperationException("Connection string 'MockDBConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<ServersideDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    connectionString = builder.Configuration.GetConnectionString("SchoolConnection")
        ?? throw new InvalidOperationException("Connection string 'SchoolConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiredAdminRole", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// Register email sender for email confirmation
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddDbContext<ServersideDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Account/Logout"))
    {
        context.Response.Cookies.Delete(".AspNetCore.Antiforgery"); // Clear antiforgery cookies on logout
        context.Response.Cookies.Delete(".AspNetCore.Identity.Application"); // Clear identity cookies
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
