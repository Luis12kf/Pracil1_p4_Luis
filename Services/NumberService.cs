using Microsoft.Data.Sqlite;
using Dapper;
using Parcil1_P4Luis.Models;

namespace Parcil1_P4Luis.Services;
    public class NumberService(IConfiguration configuration)
    {
       readonly IConfiguration _configuration = configuration; 

        private SqliteConnection CreateConnection() =>
    
                new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

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

            var parametros = new 
    {
        Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        Numero = number.Numero, // o number.Numero según cómo llamaste al record
        Resultado = number.Resultado
    };

            int filasAfectadas = await conexion.ExecuteAsync(query, parametros);
            return filasAfectadas > 0;
        }

    public async Task<NumberRecordGet?> UpdateAsync(int id, NumberRecordSet number)
        {
            const string query = @"
                UPDATE Numeros
                SET Fecha = DATETIME('now'),
                    Numero = @Numero,
                    Resultado = @Resultado
                WHERE Id = @Id";

            using var conexion = CreateConnection();

            int filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            Numero = number.Numero,
            Resultado = number.Resultado
        });

        // Retorna el registro completo actualizado usando GetByIdAsync
        return filasAfectadas > 0 ? await GetByIdAsync(id) : null;
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
