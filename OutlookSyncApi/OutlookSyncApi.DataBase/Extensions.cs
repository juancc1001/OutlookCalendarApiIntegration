using ProySuministros.DataAccess.Layer.Options;
using ProySuministros.DataAccess.Layer.Repositories.Implementations.DataBase;
using ProySuministros.DataAccess.Layer.Repositories.Interfaces;
using ProySuministros.DataAccess.Layer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ProySuministros.Business.Layer")]
namespace ProySuministros.DataAccess.Layer
{
    public static class Extensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IProductRepository,ProductRepository>();
            #region Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion
            #region Options
            services.Configure<ConfigSettingsOptions>(configuration.GetSection(ConfigSettingsOptions.Key));
            #endregion
            return services;
        }

    }
}
