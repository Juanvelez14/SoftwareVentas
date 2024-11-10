using SoftwareVentas.BLL;
using SoftwareVentas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SoftwareVentas.Data.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoftwareVentas.Data.Seeders;

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
// Registrar UserService en el contenedor de dependencias
builder.Services.AddScoped<IUsersService, UserService>();


// Identity and Access Managment
AddIAM(builder);

void AddIAM(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<User, IdentityRole>( conf =>
        {
            conf.User.RequireUniqueEmail = true;
            conf.Password.RequireDigit = false;
            conf.Password.RequiredUniqueChars = 0;
            conf.Password.RequireLowercase = false;
            conf.Password.RequireUppercase = false;
            conf.Password.RequireNonAlphanumeric = false;
            conf.Password.RequiredLength = 4;
        }).AddEntityFrameworkStores<DataContext>()
          .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(conf =>
    {
        conf.Cookie.Name = "Auth";
        conf.ExpireTimeSpan = TimeSpan.FromDays(100);
        conf.LoginPath = "/Account/Login";
        conf.AccessDeniedPath = "/Account/NotAuthorized";
    });
}



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


app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
