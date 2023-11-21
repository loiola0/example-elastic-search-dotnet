using POCELK.Services;

namespace POCELK
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IExpensedService _expensedService;

        public Worker(IExpensedService expensedService, ILogger<Worker> logger)
        {
            _expensedService = expensedService;

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _expensedService.SyncDbInformationsWithElasticSearch();

                await Task.Delay(10_000, stoppingToken);
            }
        }
    }
}