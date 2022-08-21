using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BuyWithOffer
{
    public static class ExtensionAddServices
    {

        public static void AddContextDependencyInjection(this IServiceCollection services, IConfiguration Configuration)
        {
            // db  sql server or posgre
            var dbtype = Configuration.GetConnectionString("DbType");
            if (dbtype == "SQL")
            {
                var dbConfig = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<UserDbContext>(options => options
                   .UseSqlServer(dbConfig)
                   );
            }

        }
        public static void AddServicesDependencyInjection(this IServiceCollection services)
        {


            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // mapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

        }

  
    }
}
