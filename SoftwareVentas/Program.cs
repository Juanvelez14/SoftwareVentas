using SoftwareVentas;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Seeders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios
builder.Services.AddControllersWithViews();

// Configuraci�n personalizada
builder.AddCustomBuilderConfiguration();

// Agregar configuraci�n de identidad (ya maneja la autenticaci�n)
// Reemplaza esta parte asegur�ndote de que solo se registre una vez
builder.Services.AddIdentityCore<User>(options =>
{
    // Configura las opciones necesarias, como se explic� arriba
})
    .AddRoles<IdentityRole>()  // Aseg�rate de agregar los roles si se utilizan
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Configuraci�n de autorizaci�n
builder.Services.AddAuthorization(options =>
{
    // Configurar las pol�ticas de autorizaci�n
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
    await context.Database.MigrateAsync();  // Usar versi�n asincr�nica

    // Ejecutar el seeder para roles y usuarios
    var userRolesSeeder = services.GetRequiredService<UserRolesSeeder>();
    await userRolesSeeder.SeedAsync();
}

// Configuraci�n del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configuraci�n de la autenticaci�n y autorizaci�n
app.UseAuthentication(); // Aseg�rate de que la autenticaci�n est� antes de la autorizaci�n
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddCustomWebAppConfiguration(); // Configuraci�n adicional de la app

app.Run();
