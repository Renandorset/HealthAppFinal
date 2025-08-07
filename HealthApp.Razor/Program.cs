using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthApp.Razor.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientOrAdmin", policy =>
        policy.RequireRole("Patient", "Admin"));

    options.AddPolicy("DoctorOrAdmin", policy =>
        policy.RequireRole("Doctor", "Admin"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// 2. Razor Pages + apply policies
builder.Services.AddRazorPages(options =>
{
    // ⚠️ Ordem importa: exceção primeiro!
    options.Conventions.AuthorizePage("/Doctor/Search", "PatientOrAdmin");

    // Depois restringe a pasta Doctor
    options.Conventions.AuthorizeFolder("/Doctor", "DoctorOrAdmin");

    // Outras pastas
    options.Conventions.AuthorizeFolder("/Appointments", "PatientOrAdmin");
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});

// 3. Configure EF Core and Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// 4. Build app
var app = builder.Build();

// 5. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// 6. Seed roles/users
await SeedData.Initialize(app.Services);

// 7. Request pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
