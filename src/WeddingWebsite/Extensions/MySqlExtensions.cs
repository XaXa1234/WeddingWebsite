using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeddingWebsite.Db;

namespace WeddingWebsite.Extensions
{
    public static class MySqlExtensions
    {
        public static void AddMySql(this IServiceCollection services, IConfiguration config)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0));
            string connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));
        }
    }
}
