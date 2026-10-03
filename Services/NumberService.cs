using Microsoft.Data.Sqlite;
using Dapper;
using Parcil1_P4Luis.Models;

namespace Parcil1_P4Luis.Services;
    public class NumberService
    {
        private readonly IConfiguration _configuration;

        public NumberService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqliteConnection CreateConnection() =>
            new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));

        public async Task InitializeAsync()
        {
            const string query = @"
                CREATE TABLE IF NOT EXISTS Numeros (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Fecha TEXT NOT NULL,
                    Numero INTEGER NOT NULL,
                    Resultado INTEGER NOT NULL
                );";

            using var conexion = CreateConnection();
            await conexion.ExecuteAsync(query);
        }

        public async Task<bool> SaveAsync(NumberRecordSet number)
        {
            const string query = @"INSERT INTO Numeros (Fecha, Numero, Resultado)" +
                                " VALUES (DATETIME('now'), @Numero, @Resultado)";

            using var conexion = CreateConnection();

            int filasAfectadas = await conexion.ExecuteAsync(query, number);
            return filasAfectadas > 0;
        }

        public async Task<NumberRecordSet?> UpdateAsync(int id, NumberRecordGet number)
        {
            const string query = @"
                UPDATE Numeros
                SET Fecha = DATETIME('now'),
                    Numero = @Numero,
                    Resultado = @Resultado
                WHERE Id = @Id";

            using var conexion = CreateConnection();
            int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id, number.Numero, number.Resultado });

            return filasAfectadas > 0 ? await GetByIdSetAsync(id) : null;
        }

        public async Task<NumberRecordGet?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT Id, Fecha, Numero, Resultado
                FROM Numeros WHERE Id = @Id";

            using var conexion = CreateConnection();
            return await conexion.QueryFirstOrDefaultAsync<NumberRecordGet>(query, new { Id = id });
        }

        private async Task<NumberRecordSet?> GetByIdSetAsync(int id)
        {
            const string query = @"
                SELECT Id, Fecha, Numero, Resultado
                FROM Numeros WHERE Id = @Id";

            using var conexion = CreateConnection();
            return await conexion.QueryFirstOrDefaultAsync<NumberRecordSet>(query, new { Id = id });
        }

        public async Task<IEnumerable<NumberRecordGet>> GetListAsync()
        {
            const string query = "SELECT Id, Fecha, Numero, Resultado FROM Numeros";

            using var conexion = CreateConnection();
            return await conexion.QueryAsync<NumberRecordGet>(query);
        }

    
    }
