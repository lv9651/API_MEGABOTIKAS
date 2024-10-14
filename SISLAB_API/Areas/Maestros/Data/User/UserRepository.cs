using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class UserRepository
{
    private readonly string? _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = new List<User>();

        using (var connection = CreateConnection()) // Usar el método de crear conexión
        {
            await connection.OpenAsync();

            string query = @"
                SELECT u.id, u.dni, u.username, u.password, r.name AS role,u.NOMBRE as NOMBRE
                FROM users u
                LEFT JOIN roles r ON u.role_id = r.id";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            id = reader.GetInt32("id"),
                            dni = reader.GetString("dni"),
                            username = reader.GetString("username"),
                            password = reader.GetString("password"),
                            role = reader.GetString("role"),
                            NOMBRE = reader.GetString("NOMBRE")
                        });
                    }
                }
            }
        }

        return users;
    }







    public async Task<IEnumerable<Role>> GetAllRolAsync()
    {
        var Roles = new List<Role>();

        using (var connection = CreateConnection()) // Usar el método de crear conexión
        {
            await connection.OpenAsync();

            string query = @"
                SELECT * FROM roles";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Roles.Add(new Role
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name")
                         
                        });
                    }
                }
            }
        }

        return Roles;
    }
}