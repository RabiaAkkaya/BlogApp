using Microsoft.AspNetCore.Identity.UI.Services;

// ...

public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.AddSingleton<IEmailSender, YourEmailSenderImplementation>();

    services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Diğer Identity konfigürasyonları...
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // ...
}
