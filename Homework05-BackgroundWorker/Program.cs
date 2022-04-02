using Homework05_Business.Abstracts;
using Homework05_Business.Concretes;
using Homework05_DataAccess.Entityframework.Repository.Abstracts;
using Homework05_DataAccess.Entityframework.Repository.Concretes;
using Homework05_DataAccess.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Homework05_BackgroundWorker
{
    public class Program
    {
        //public IConfiguration Configuration { get; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>()/* .AddDbContext<AppDbContext>(options => options.UseSqlServer(ConfigSettings.dbSQLEXPRESS)) */
                    .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                    .AddTransient<IUnitOfWork, UnitOfWork>()
                    .AddTransient<IUserService, UserService>()
                    .AddHostedService<Worker>();


                });


          
    }
}
