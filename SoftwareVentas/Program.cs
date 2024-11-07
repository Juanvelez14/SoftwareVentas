using SoftwareVentas.BLL;
using SoftwareVentas.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Configurar la ruta de las vistas en la carpeta "Presentation/Views"
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Presentation/Views/{1}/{0}.cshtml");  // Ruta para vistas
        options.ViewLocationFormats.Add("/Presentation/Views/Shared/{0}.cshtml");  // Ruta para vistas compartidas
    });

// Registrar Data Context para la base de datos
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});

// Registrar CustomerService en el contenedor de dependencias
builder.Services.AddScoped<CustomerService>();

// Registrar ProductService en el contenedor de dependencias
builder.Services.AddScoped<ProductService>(); // Asegúrate de que esto esté registrado correctamente

// Configurar otros servicios si es necesario

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
