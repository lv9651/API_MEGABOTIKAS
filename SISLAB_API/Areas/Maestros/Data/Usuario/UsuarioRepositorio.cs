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
        _connectionString = configuration.GetConnectionString("SqlServerQFPHARMA")
            ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    // Registrar un usuario y devolver el Id generado
    public async Task<SocialLoginResult> SocialLoginAsync(Usuario usuario)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Nombre", usuario.Nombre);
        parameters.Add("@ApellidoPaterno", usuario.ApellidoPaterno);
        parameters.Add("@ApellidoMaterno", usuario.ApellidoMaterno);
        parameters.Add("@Correo", usuario.Correo);
        parameters.Add("@TipoDocumento", usuario.TipoDocumento);
        parameters.Add("@NumeroDocumento", usuario.NumeroDocumento);

        // Ejecutamos el SP y leemos la primera fila
        var row = await Connection.QueryFirstOrDefaultAsync<dynamic>(
            "[clubqf].[sp_SocialLogin]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        // Si el SP devolvió columna "Mensaje", significa que hubo error/usuario existente
        if (row != null && row.Mensaje != null)
        {
            return new SocialLoginResult { Mensaje = row.Mensaje, Usuario = null };
        }

        // Si devolvió datos del usuario
        var usuarioResult = row != null ? new Usuario
        {
            IdUsuario = row.IdUsuario,
            Nombre = row.Nombre,
            ApellidoPaterno = row.ApellidoPaterno,
            ApellidoMaterno = row.ApellidoMaterno,
            Correo = row.Correo,
            TipoDocumento = row.TipoDocumento,
            NumeroDocumento = row.NumeroDocumento,
            FechaRegistro = row.FechaRegistro
        } : null;

        return new SocialLoginResult { Mensaje = null, Usuario = usuarioResult };
    }




    public async Task<UsuarioDto?> ExisteCorreoAsync(string correo)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("clubqf.sp_ValidarCorreo", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Correo", correo);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new UsuarioDto
            {
                Nombre = reader["Nombre"] as string,
                ApellidoPaterno = reader["ApellidoPaterno"] as string,
                ApellidoMaterno = reader["ApellidoMaterno"] as string,
                Correo = reader["Correo"] as string,
                TipoDocumento = reader["TipoDocumento"] as string,
                NumeroDocumento = reader["NumeroDocumento"] as string
            };
        }

        return null;
    }
    // Llama al stored procedure sp_ObtenerUltimaCompraCliente
    public async Task<CompraResult?> GetUltimaCompraPorDocumentoAsync(string nroDocumento)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@nroDocumento", nroDocumento, DbType.String);

        // Si tu procedure devuelve las columnas UltimaFechaCompra y MontoTotal
        var result = await Connection.QueryFirstOrDefaultAsync<CompraResult>(
            "[clubqf].[sp_ObtenerUltimaCompraCliente]", // ajusta schema/proc si corresponde
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result;
    }


// Guardar código de recuperación
public async Task<int> GuardarCodigoRecuperacion(string correo, string codigo, DateTime expiracion)
    {
        using var db = Connection;
        return await db.ExecuteAsync(
            "clubvinali.sp_GuardarCodigoRecuperacion_", // nombre correcto del SP
            new
            {
                Correo = correo,
                Codigo = codigo,
                Expiracion = expiracion
            },
            commandType: CommandType.StoredProcedure
        ).ConfigureAwait(false);
    }

    // Actualizar contraseña
    public async Task<int> ActualizarContrasena(string correo, string nuevaHash)
    {
        using var db = Connection;
        return await db.ExecuteAsync(
            "clubvinali.sp_ActualizarContrasena",
            new
            {
                Correo = correo,
                NuevaHash = nuevaHash
            },
            commandType: CommandType.StoredProcedure
        ).ConfigureAwait(false);
    }
}