using Homework05_BackgroundWorker.DTO;
using Homework05_Business.Abstracts;
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

                    IQueryable<UserDTO> cameUsers = result.AsQueryable();//sistemi yormamak için queryable olarak join yapacaðým.
                    IQueryable<User> users = _userService.GetAllUserAsQueryable();


                    var crossJoinResult = from cameUser in cameUsers
                                          from user in users
                                          select new
                                          {
                                              CameUser = cameUser,
                                              User = user
                                          };
                    //json içinde bulunan ancak veritabanýnda bulunmayan veriler. Bunlarý Veritabanýna eklemeliyiz.
                    var addusers = crossJoinResult
                                    .Where(x => x.User == null)
                                    .Select(x => new User()
                                    {
                                        Id = x.CameUser.Id,
                                        UserId = x.CameUser.UserId,
                                        Body = x.CameUser.Body,
                                        Title = x.CameUser.Title
                                    }
                                            ).ToList();
                    //Hem json içinde hemde veritabanýnda bulunan veriler. Bunlarý Veritabanýna güncellemeliyiz Jsondan nasýl geliyorlarsa.
                    var updateusers = crossJoinResult
                                    .Where(x => x.User != null && x.CameUser !=null)
                                    .Select(x => new User()
                                    {
                                        Id = x.CameUser.Id,
                                        UserId = x.CameUser.UserId,
                                        Body = x.CameUser.Body,
                                        Title = x.CameUser.Title
                                    }
                                            ).ToList();


                    //Json da olup, veritabanýnda olmayan veriler.
                    var deleteusers = crossJoinResult
                                    .Where(x => x.CameUser == null)
                                    .Select(x => x.User).ToList();


                    //Veritabaný iþlemleri burada yapýlýyor.
                    _userService.AddUsers(addusers);
                    _userService.UpdateUsers(updateusers);
                    _userService.DeleteUsers(deleteusers);
                     
                }
                else
                {
                    _logger.LogError(@"https://jsonplaceholder.typicode.com/posts is down Status Code {StatusCode}", response.StatusCode);
                }
                /*1 dakika bekle, sonra devam et. 1 dakikada bir çalýþmasý için!*/
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
