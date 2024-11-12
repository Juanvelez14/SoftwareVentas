using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Helpers;
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
            });

            // Services
            AddServices(builder);

            // Identity and Acces Managment

            // Toast Notification
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }
        public static void AddServices(WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            // Helpers
        }

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }


    }
}
