using Homework05_BackgroundWorker.Helpers;
using Homework05_Business.Abstracts;
using Homework05_Business.Concretes;
using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_DataAccess.Entityframework.Repository.Concretes;
using Homework05_DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework05_BackgroundWorker
{
    public class Program
    {
        //public IConfiguration Configuration { get; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    //services.AddDbContext<AppDbContext>(ServiceLifetime.Transient);


                    /*services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConfigSettings.dbSQLEXPRESS));

                    services.AddTransient<IUnitOfWork, UnitOfWork>();
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient<IUserService, UserService>();*/

                    services.AddHostedService<Worker>();
                });
    }
}
