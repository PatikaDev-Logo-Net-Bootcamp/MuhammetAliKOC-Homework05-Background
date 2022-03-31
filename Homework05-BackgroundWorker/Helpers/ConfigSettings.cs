using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework05_BackgroundWorker.Helpers
{
    public static class ConfigSettings
    {
        public static string dbSQLEXPRESS { get; }
        static ConfigSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            dbSQLEXPRESS = configurationBuilder.Build().GetSection("ConnectionStrings:DBSQLEXPRESS").Value;
        }
    }
}
