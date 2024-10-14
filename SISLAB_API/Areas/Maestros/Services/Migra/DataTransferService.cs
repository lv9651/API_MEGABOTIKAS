using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;

public class DataTransferService
{
    private readonly string _sqlServerConnectionString;
    private readonly string _mysqlConnectionString;

    public DataTransferService(IConfiguration configuration)
    {
        _sqlServerConnectionString = configuration.GetConnectionString("SqlServersige");
        _mysqlConnectionString = configuration.GetConnectionString("SislabConnection");
    }

    public async Task TransferDataAsync()
    {
        try
        {
            using (var sqlConnection = new SqlConnection(_sqlServerConnectionString))
            {
                await sqlConnection.OpenAsync();

                var sqlQuery = "SELECT documento as dni, userName, clave, nombres " +
                "FROM (SELECT documento, userName, clave, CONCAT(nombres, ' ', ape_paterno) as nombres, " +
                "ROW_NUMBER() OVER (PARTITION BY documento ORDER BY documento) AS ord " +
                "FROM principal.empleado WHERE estado = 'HABILITADO') d " +
                "WHERE ord = 1 AND userName NOT IN ('', 'PRUEBA')";

                using (var command = new SqlCommand(sqlQuery, sqlConnection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    using (var mysqlConnection = new MySqlConnection(_mysqlConnectionString))
                    {
                        await mysqlConnection.OpenAsync();

                        while (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                dni = reader.GetString(0),
                                Username = reader.GetString(1),
                                clave = reader.GetString(2),
                                nombres = reader.GetString(3),
                            };

                            var insertQuery = "INSERT INTO users (dni, username, password,role_id,NOMBRE) VALUES (@dni, @Username, @clave,1,@nombres)";
                            using (var insertCommand = new MySqlCommand(insertQuery, mysqlConnection))
                            {
                                insertCommand.Parameters.AddWithValue("@dni", usuario.dni);
                                insertCommand.Parameters.AddWithValue("@Username", usuario.Username);
                                insertCommand.Parameters.AddWithValue("@clave", usuario.clave);
                                insertCommand.Parameters.AddWithValue("@nombres", usuario.nombres);

                                await insertCommand.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
        }
        catch (SqlException sqlEx)
        {
            // Manejo específico de errores de SQL Server
            throw new InvalidOperationException($"Error al acceder a SQL Server: {sqlEx.Message}", sqlEx);
        }
        catch (MySqlException mySqlEx)
        {
            // Manejo específico de errores de MySQL
            throw new InvalidOperationException($"Error al acceder a MySQL: {mySqlEx.Message}", mySqlEx);
        }
        catch (Exception ex)
        {
            // Manejo general de otros errores
            throw new InvalidOperationException($"Error inesperado: {ex.Message}", ex);
        }
    }
}