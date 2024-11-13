using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Helpers;
using SoftwareVentas.Data.Seeders;
using SoftwareVentas.Services;
using AspNetCoreHero.ToastNotification.Extensions;
using SoftwareVentas;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios de la aplicación
builder.AddCustomBuilderConfiguration(); // Llama al método de configuración personalizada

// Agregar servicios de controladores y vistas
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRoleService, RoleService>();


var app = builder.Build();

// Llamar al seeder para inicializar los roles y usuarios si es necesario
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<UserRolesSeeder>();
    try
    {
        await seeder.SeedAsync();  // Llama al método para crear roles y usuarios
    }
    catch (Exception ex)
    {
        // Maneja cualquier error que pueda ocurrir al hacer el seeding
        Console.Error.WriteLine($"Error al inicializar roles y usuarios: {ex.Message}");
    }
}

// Configuración de middlewares
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Usar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Usar Toast Notifications
app.UseNotyf();

// Configurar rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
