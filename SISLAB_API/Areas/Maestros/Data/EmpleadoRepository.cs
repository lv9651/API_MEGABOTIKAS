using Dapper;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class EmpleadoRepository
{
    private readonly string? _connectionString;

    public EmpleadoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServerConnection");
    }

    private SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<IEnumerable<Empleado>> ObtenerEmpleadosAsync()
    {
        using (var connection = CreateConnection())
        {
            var query = "obtener_empleados"; // Asegúrate de que este procedimiento exista en SQL Server
            return await connection.QueryAsync<Empleado>(query, commandType: CommandType.StoredProcedure);
        }
    }
}