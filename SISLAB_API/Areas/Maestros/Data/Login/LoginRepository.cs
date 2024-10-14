using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class LoginRepository
{
    private readonly string? _connectionString;

    public LoginRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }


    public async Task<Login> AuthenticateUserAsync(string username, string password)
    {
        using (var connection = CreateConnection())
        {
            var query = "AuthenticateUser";
            var parameters = new DynamicParameters();
            parameters.Add("p_Username", username, DbType.String);
            parameters.Add("p_Password", password, DbType.String);

            var result = await connection.QuerySingleOrDefaultAsync<Login>(
                query,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
    public async Task AddAsync(Usuario usuario)
    {
        using (var connection = CreateConnection())
        {
            var query = "INSERT INTO users (dni, username, password, role_id) VALUES (@dni, @Username, @clave, @role_id)";
            var parameters = new
            {
                dni = usuario.dni,
                Username = usuario.Username,
                clave = usuario.clave,
                role_id = usuario.role_id // Asegúrate de que esta propiedad exista en el modelo Usuario
            };

            await connection.ExecuteAsync(query, parameters);
        }
    }
}