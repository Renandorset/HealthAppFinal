// 1. Startup configuration (Program.cs ou Startup.cs depending on .NET version)

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthApp.Razor.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientOrAdmin", policy =>
        policy.RequireRole("Patient", "Admin"));

    options.AddPolicy("DoctorOrAdmin", policy =>
        policy.RequireRole("Doctor", "Admin"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// 2. Razor page restrictions
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Appointments", "PatientOrAdmin");
    options.Conventions.AuthorizeFolder("/Doctor", "DoctorOrAdmin");
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});

// 3. Identity and EF Core
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

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

await SeedData.Initialize(app.Services);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
