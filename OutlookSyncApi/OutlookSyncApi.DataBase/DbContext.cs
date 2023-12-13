using ProySuministros.DataAccess.Layer.Configurations;
using ProySuministros.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProySuministros.DataAccess.Layer
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Product> Productos => Set<Product>();
        private readonly string _connectionString;
        public DbContext(DbContextOptions<DbContext> options,
                                 IConfiguration configuration) : base(options) 
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductConfiguration());
        }

    }
}
