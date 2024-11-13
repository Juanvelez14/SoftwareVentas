using SoftwareVentas;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Seeders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllersWithViews();

// Configuración personalizada
builder.AddCustomBuilderConfiguration();

// Agregar configuración de identidad (ya maneja la autenticación)
// Reemplaza esta parte asegurándote de que solo se registre una vez
builder.Services.AddIdentityCore<User>(options =>
{
    // Configura las opciones necesarias, como se explicó arriba
})
    .AddRoles<IdentityRole>()  // Asegúrate de agregar los roles si se utilizan
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Configuración de autorización
builder.Services.AddAuthorization(options =>
{
    // Configurar las políticas de autorización
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Empleado"));
});

WebApplication app = builder.Build();

// Ejecutar migraciones pendientes y el seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Obtener el contexto de la base de datos
    var context = services.GetRequiredService<DataContext>();

    // Aplicar migraciones pendientes
    await context.Database.MigrateAsync();  // Usar versión asincrónica

    // Ejecutar el seeder para roles y usuarios
    var userRolesSeeder = services.GetRequiredService<UserRolesSeeder>();
    await userRolesSeeder.SeedAsync();
}

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configuración de la autenticación y autorización
app.UseAuthentication(); // Asegúrate de que la autenticación está antes de la autorización
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddCustomWebAppConfiguration(); // Configuración adicional de la app

app.Run();
