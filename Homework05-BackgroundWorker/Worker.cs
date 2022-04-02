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

                    var cameUsers = result;
                    var users = _userService.GetAllUserAsQueryable().ToList();

                    var leftJoinForAdd = from cameUser in cameUsers 
                                          join user in users on cameUser.Id equals user.Id  
                                          into total
                                          from userLeft in total.DefaultIfEmpty()
                                          select new
                                          {
                                              CameUser = cameUser,
                                              User = userLeft
                                          };

                    //json içinde bulunan ancak veritabanýnda bulunmayan veriler. Bunlarý Veritabanýna eklemeliyiz.
                    var addusers = leftJoinForAdd
                                    .Where(x => x.User == null)
                                    .Select(x => new User()
                                    {
                                        Id = x.CameUser.Id,
                                        UserId = x.CameUser.UserId,
                                        Body = x.CameUser.Body,
                                        Title = x.CameUser.Title
                                    }
                                    ).ToList();




                    var joinForUpdate = from cameUser in cameUsers
                                         join user in users on cameUser.Id equals user.Id
                                         select new
                                         {
                                             CameUser = cameUser,
                                             User = user
                                         };

                    //Hem json içinde hemde veritabanýnda bulunan veriler. Bunlarý Veritabanýna güncellemeliyiz Jsondan nasýl geliyorlarsa.
                    var updateusers = joinForUpdate
                                    .Where(x => x.User != null && x.CameUser != null)
                                    /*.Select(x => new User()
                                    {
                                        Id = x.CameUser.Id,
                                        UserId = x.CameUser.UserId,
                                        Body = x.CameUser.Body,
                                        Title = x.CameUser.Title
                                    })*/
                                    .Select( x=> { x.User.UserId = x.CameUser.UserId; x.User.Title = x.CameUser.Title; x.User.Body = x.CameUser.Body; return x.User; } )
                                            .ToList();


                    var leftJoinForDelete = from user in users
                                            join cameUser in cameUsers on user.Id equals cameUser.Id
                                         into total
                                         from userLeft in total.DefaultIfEmpty()
                                         select new
                                         {
                                             CameUser = userLeft,
                                             User = user
                                         };


                    //Json da olup, veritabanýnda olmayan veriler.
                    var deleteusers = leftJoinForDelete
                                    .Where(x => x.CameUser == null)
                                    .Select(x => x.User).ToList();



                    _userService.AddUsers(addusers);
                    _userService.UpdateUsers(updateusers);
                    _userService.DeleteUsers(deleteusers);

                }
                else
                {
                    _logger.LogError(@"https://jsonplaceholder.typicode.com/posts is down Status Code {StatusCode}", response.StatusCode);
                }
                //1 dakika bekle, sonra devam et. 1 dakikada bir çalýþmasý için!
                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}
