using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Helpers;
using SoftwareVentas.Data.Seeders;
using SoftwareVentas.Services;


namespace SoftwareVentas
{
    public static class CustomConfigutarion
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
                //configuration.UseSqlServer(builder.Configuration.GetConnectionString("MiConexionLocal"));
            });

            builder.Services.AddHttpContextAccessor();

            // Services
            AddServices(builder);

            // Identity and Acces Managment
            AddIAM(builder);
            // Toast Notification
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }
        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
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
            // Registrar las políticas de autorización

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Empleado"));
            });


            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Account/NotAuthorized";
            });
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ISaleService, SaleService>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddTransient<SeederDB>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            //Helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
        }

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }



        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory!.CreateScope())
            {
                SeederDB service = scope.ServiceProvider.GetService<SeederDB>();
                service!.SeedAsync().Wait();
            }
        }


    }
}