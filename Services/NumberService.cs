using Microsoft.Data.Sqlite;
using Dapper;

namespace Parcil1_P4Luis.Services;
    public class NumberService
    {
        private readonly string _connectionString;

        public NumberService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> GetNumberAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleAsync<int>("SELECT number FROM Numbers LIMIT 1");
                return result;
            }
        }
    }
