using Dapper;
using MySql.Data.MySqlClient; // Use MySqlConnection for MySQL
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SISLAB_API.Areas.Maestros.Models;


    public class BloqueAnuncioRepository
    {
        private readonly string _connectionString;

        // Constructor to inject IConfiguration and get the connection string
        public BloqueAnuncioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SislabConnection");
        }

        // Define a property to get a MySQL connection
        private IDbConnection Connection => new MySqlConnection(_connectionString);

        // Get a single block by ID
        public async Task<BloqueAnuncio> GetByIdAsync(int id)
        {
            using (var connection = Connection)
            {
                var query = "SELECT * FROM Bloque_Anuncio WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<BloqueAnuncio>(query, new { Id = id });
            }
        }

        // Get all blocks
        public async Task<IEnumerable<BloqueAnuncio>> GetAllAsync()
        {
            using (var connection = Connection)
            {
                var query = "SELECT * FROM Bloque_Anuncio";
                return await connection.QueryAsync<BloqueAnuncio>(query);
            }
        }

        // Create a new block
        public async Task<int> CreateAsync(BloqueAnuncio bloqueAnuncio)
        {
            using (var connection = Connection)
            {
                var query = "INSERT INTO Bloque_Anuncio (Descripcion) VALUES (@Descripcion)";
                return await connection.ExecuteAsync(query, bloqueAnuncio);  // Returns the number of rows affected
            }
        }

        // Update an existing block
        public async Task<int> UpdateAsync(BloqueAnuncio bloqueAnuncio)
        {
            using (var connection = Connection)
            {
                var query = "UPDATE Bloque_Anuncio SET Descripcion = @Descripcion WHERE Id = @Id";
                return await connection.ExecuteAsync(query, bloqueAnuncio);  // Returns the number of rows affected
            }
        }

        // Delete a block by ID
        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = Connection)
            {
                var query = "DELETE FROM Bloque_Anuncio WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });  // Returns the number of rows affected
            }
        }
    }
