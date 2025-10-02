using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

public class UsuarioRepositorio
{
    private readonly string _connectionString;

    public UsuarioRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MegaBotikasConnection")
            ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
    }
    public async Task<bool> validarUsuario_Login(string usuario, string dni)
    {
        using var connection = new SqlConnection(_connectionString);

        var parametros = new DynamicParameters();
        parametros.Add("@usuario", usuario);
        parametros.Add("@dni", dni);
        parametros.Add("@respuesta", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("usuario.sp_validarUsuario_Login", parametros, commandType: CommandType.StoredProcedure);

        return parametros.Get<bool>("@respuesta");
    }
}