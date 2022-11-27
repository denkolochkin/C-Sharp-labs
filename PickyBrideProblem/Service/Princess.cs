using PickyBrideProblem.Entity;
using PickyBrideProblem.Dto;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Text;
using System.Net.Http.Json;

using MassTransit;

using static Npgsql.PostgresTypes.PostgresCompositeType;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

//todo среднее по 100 случайных попыток

namespace PickyBrideProblem.Service
{
    public class Princess : IHostedService
    {
        private readonly int ContendersCount = 100;

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
                );

        private readonly IHostApplicationLifetime _appLifetime;
        private IBusControl? _busControl;

        private DataBaseContext _dataBaseContext;
       
        private ContenderConsumer _contenderConsumer;

        private List<ContenderDto> FirstContenders = new();
        private ContenderDto? BestOfFirst = null;

        private int DatesCount = 1;

        private List<int> Results = new();

        private static HttpClient Client = new()
        {
            BaseAddress = new Uri("https://localhost:7132"),
        };

        public Princess(IHostApplicationLifetime appLifetime,
            Hall hall, Friend friend, ContenderGenerator generator,
            DataBaseContext dataBaseContext, ContenderConsumer contenderConsumer)
        {
            _appLifetime = appLifetime;
            _dataBaseContext = dataBaseContext;
            _contenderConsumer = contenderConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("contender-event", e =>
                {
                    e.Consumer<ContenderConsumer>();
                });
            });
            _busControl.StartAsync(new CancellationToken());

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        FirstContenders = new();
                        DatesCount = 1;
                        BestOfFirst = null;
                        await ReprocessAttempt(i, _appLifetime);
                    }
                    Console.WriteLine("Average Quality - " + Results.Sum() / Results.Count());
                    _busControl.StopAsync();
                    _appLifetime.StopApplication();

                });
            });

            return Task.CompletedTask;
        }

        private async Task<int> ReprocessAttempt(int number, IHostApplicationLifetime lifetime)
        {
            int result = 0;
            for (int i = 0; i < ContendersCount; i++)
            {
                await Client.GetAsync("hall/" + number + "/next");

                Thread.Sleep(300);

                ContenderDto contenderDto = await ContenderConsumer.GetNext();

                LogStatus(contenderDto);

                if (DatesCount < (ContendersCount / Math.E))
                {
                    FirstContenders.Add(contenderDto);
                }
                else
                {
                    ContenderDto bestOfFirst = await FindBestOfFirst(number);
                    ContenderDto winner = await compareContenders(contenderDto, bestOfFirst, number);

                    if (!winner.Quality.Equals(bestOfFirst.Quality))
                    {
                        LogResult(contenderDto);
                        result = contenderDto.Quality;
                        DatesCount++;
                        break;
                    }
                    if (DatesCount.Equals(ContendersCount))
                    {
                        result = 10;
                        LogLastResult();
                        break;
                    }
                }

                DatesCount++;
            }
            return result;
        }

        private async Task<ContenderDto> FindBestOfFirst(int number)
        {
            if (BestOfFirst != null)
            {
                return BestOfFirst;
            }

            ContenderDto bestContenter = FirstContenders.First();
            for (int i = 1; i < FirstContenders.Count; i++)
            {
                bestContenter = await compareContenders(bestContenter, FirstContenders[i], number);
            }

            BestOfFirst = bestContenter;
            return BestOfFirst;
        }

        private async Task<ContenderDto> compareContenders(ContenderDto first, ContenderDto second, int number)
        {
           HttpResponseMessage responseMessage =
                await Client.PostAsJsonAsync(
                    "friend/" + number + "/compare", new ComparingDto(first.FirstName, first.LastName, second.FirstName, second.LastName)
                );

            ContenderDto? winnerDto = await responseMessage.Content.ReadFromJsonAsync<ContenderDto>();

            return winnerDto;
        }

        private void LogStatus(ContenderDto contender)
        {
            log.Info("Date #" + DatesCount
                + " : " + contender.FirstName + " " + contender.LastName);
        }

        private void LogResult(ContenderDto contender)
        {
            if (contender.Quality <= 50)
            {
                Results.Add(0);
                log.Info("Result: 0 Dates count: " + DatesCount);
                return;
            }
            Results.Add(contender.Quality);
            log.Info("Result: " + contender.Quality + " Dates count: " + DatesCount);
        }

        private void LogLastResult()
        {
            Results.Add(10);
            log.Info("Result: 10 Dates count: " + DatesCount);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

