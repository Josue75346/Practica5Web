using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;
using SistemaFarmacia.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// 1. CONEXIÓN A SQL SERVER
// ======================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ======================================================
// 2. CONFIGURACIÓN DE IDENTITY
// ======================================================

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // PASSWORD
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // BLOQUEO
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // USUARIO
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// ======================================================
// 3. POLÍTICAS DE AUTORIZACIÓN
// ======================================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin",
        policy => policy.RequireRole("Administrador"));

    options.AddPolicy("SoloFarmaceutico",
        policy => policy.RequireRole("Farmaceutico"));

    options.AddPolicy("SoloCliente",
        policy => policy.RequireRole("Cliente"));

    options.AddPolicy("AdminFarmaceutico",
        policy => policy.RequireRole("Administrador", "Farmaceutico"));

    // Policy basada en CLAIM
    options.AddPolicy("PuedeVerReportes",
        policy => policy.RequireClaim("PermisoReportes", "true"));
});


// ======================================================
// 4. SERVICIOS
// ======================================================

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuditoriaService>();

builder.Services.AddScoped<BitacoraService>();


// ======================================================
// 5. CONFIGURACIÓN DE COOKIE (LOGIN / LOGOUT / PERMISOS)
// ======================================================

builder.Services.ConfigureApplicationCookie(options =>
{
    // Página de login
    options.LoginPath = "/Auth/Login";

    // Página si no tiene permisos
    options.AccessDeniedPath = "/Home/SinPermisos";

    // Auditoría LOGIN
    options.Events.OnSigningIn = async ctx =>
    {
        var auditoria = ctx.HttpContext.RequestServices
            .GetRequiredService<AuditoriaService>();

        await auditoria.RegistrarAsync("Inicio de sesión", "Auth");
    };

    // Auditoría LOGOUT
    options.Events.OnSigningOut = async ctx =>
    {
        var auditoria = ctx.HttpContext.RequestServices
            .GetRequiredService<AuditoriaService>();

        await auditoria.RegistrarAsync("Cierre de sesión", "Auth");
    };
});


// ======================================================
// 6. CONSTRUIR APP
// ======================================================

var app = builder.Build();


// ======================================================
// 7. CREAR ROLES AUTOMÁTICAMENTE
// ======================================================

async Task CrearRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Administrador", "Farmaceutico", "Cliente" };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    // ======================================================
    // AGREGAR CLAIM AL ADMIN
    // ======================================================

    var admin = await userManager.FindByEmailAsync("admin@gmail.com");

    if (admin != null)
    {
        var claims = await userManager.GetClaimsAsync(admin);

        if (!claims.Any(c => c.Type == "PermisoReportes"))
        {
            await userManager.AddClaimAsync(admin,
                new Claim("PermisoReportes", "true"));
        }
    }
}


// Ejecutar creación de roles
await CrearRolesAsync(app);


// ======================================================
// 8. MIDDLEWARE
// ======================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


// ======================================================
// 9. RUTAS
// ======================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();