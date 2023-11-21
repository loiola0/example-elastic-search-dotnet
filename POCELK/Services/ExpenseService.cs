using Nest;
using POCELK.Entities;
using POCELK.Repositories;

namespace POCELK.Services
{
    public class ExpensedService : IExpensedService
    {
        private readonly IElasticClient _elasticClient;

        private readonly IConfiguration _configuration;

        private readonly IExpensedRepository _expensedRepository;

        private readonly ILogger<Worker> _logger;

        public ExpensedService(IElasticClient elasticClient,
                              IConfiguration configuration,
                              IExpensedRepository expensedRepository,
                              ILogger<Worker> logger)
        {
            _elasticClient = elasticClient;

            _configuration = configuration;

            _expensedRepository = expensedRepository;

            _logger = logger;
        }

        public async Task SyncDbInformationsWithElasticSearch()
        {
            var expenseds = await _expensedRepository.GetAllExpense();

            if (expenseds.Count == 0)
                _logger.LogInformation("There is no data to be entered!");

            _ = InsertAllDocumentsIntoTheIndexSuccessfully(expenseds);
        }

        private bool InsertAllDocumentsIntoTheIndexSuccessfully(List<Expense> expenses)
        {
            var indexExpense = _configuration.GetValue<string>("ElasticSettings:IndexGastos");

            bool result = true;

            expenses.ForEach(async g =>
            {
                var response = await _elasticClient.IndexAsync(g, idx => idx.Index(indexExpense));

                if (!response.IsValid)
                {
                    _logger.LogInformation($"Error during insertion: ", response.DebugInformation);

                    result = false;
                }
            });

            if (result)
                _logger.LogInformation("All data was entered successfully");

            return result;
        }
    }
}
