using MySqlConnector;
using POCELK.Entities;

namespace POCELK.Repositories
{
    public class ExpenseRepository : IExpensedRepository
    {
        public IConfiguration _configuration { get; set; }

        public ExpenseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Expense>> GetAllExpense()
        {
            var connectionString = _configuration.GetValue<string>("ConnectionString");

            var connection = new MySqlConnection(connectionString);

            await connection.OpenAsync();

            var query = "SELECT *FROM Gastos";

            using var cmd = new MySqlCommand(query, connection);

            using var reader = await cmd.ExecuteReaderAsync();

            var result = await ExtractDbDataToExpenseList(reader);

            return result;
        }

        private static async Task<List<Expense>> ExtractDbDataToExpenseList(MySqlDataReader reader)
        {
            int id;

            decimal value;

            DateTime lastUpdateDate;

            List<Expense> result = new();

            while (await reader.ReadAsync())
            {
                id = reader.GetInt32(reader.GetOrdinal("Id"));

                value = reader.GetDecimal(reader.GetOrdinal("Valor"));

                lastUpdateDate = reader.GetDateTime(reader.GetOrdinal("UltimaModificacao"));

                result.Add(new Expense(id, value, lastUpdateDate));
            }

            return result;
        }
    }
}
