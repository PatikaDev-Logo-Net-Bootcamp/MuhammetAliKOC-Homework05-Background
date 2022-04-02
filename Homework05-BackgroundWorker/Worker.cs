using Homework05_Business.Abstracts;
using Homework05_Business.DTO;
using Homework05_Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Homework05_BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient httpClient;
        private readonly IUserService _userService;

        public Worker(ILogger<Worker> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("https://jsonplaceholder.typicode.com/posts is up Status Code {StatusCode}", response.StatusCode);
                    var userJson = await response.Content.ReadAsStringAsync();
                    List<UserDTO> result = JsonConvert.DeserializeObject<List<UserDTO>>(userJson);

                    var cameUsers = result;
                     //eklenecek kayýtlarý bul ve ekle
                    _userService.AddUsers(cameUsers);
                    //Güncellenmesi gereken kayýtlarý güncelle
                    _userService.UpdateUsers(cameUsers);
                    //Silinmesi gereken kayýtlarý sil
                    _userService.DeleteUsers(cameUsers);

                }
                else
                {
                    _logger.LogError(@"https://jsonplaceholder.typicode.com/posts is down Status Code {StatusCode}", response.StatusCode);
                }
                //1 dakika bekle, sonra devam et. 1 dakikada bir çalýþmasý için! 1 saniye için 1000 kullanýlýr.
                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}
