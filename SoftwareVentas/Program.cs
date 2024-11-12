using SoftwareVentas;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SoftwareVentas.Data.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoftwareVentas.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.AddCustomBuilderConfiguration();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//AddIAM(builder);

//void AddIAM(WebApplicationBuilder builder)
//{
//	// Configurar Identity y Roles
//	builder.Services.AddIdentity<User, IdentityRole>(conf =>
//	{
//		conf.User.RequireUniqueEmail = true;
//		conf.Password.RequireDigit = false;
//		conf.Password.RequiredUniqueChars = 0;
//		conf.Password.RequireLowercase = false;
//		conf.Password.RequireUppercase = false;
//		conf.Password.RequireNonAlphanumeric = false;
//		conf.Password.RequiredLength = 4;
//	})
//	.AddEntityFrameworkStores<DataContext>()
//	.AddDefaultTokenProviders();
//	// Registrar las políticas de autorización
//	builder.Services.AddAuthorization(options =>
//	{
//		options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
//		options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Empleado"));
//	});

//	// Configuración de cookies
//	builder.Services.ConfigureApplicationCookie(conf =>
//	{
//		conf.Cookie.Name = "Auth";
//		conf.ExpireTimeSpan = TimeSpan.FromDays(100);
//		conf.LoginPath = "/Account/Login";
//		conf.AccessDeniedPath = "/Account/NotAuthorized";
//	});
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddCustomWebAppConfiguration();

app.Run();