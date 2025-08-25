using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Data;
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
    public async Task<int> RegistrarUsuario(UsuarioVinali usuario)
    {
        using var db = Connection;

        // 🔑 Hashear la contraseña en texto plano
        string hash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

        return await db.ExecuteScalarAsync<int>(
            "clubvinali.sp_RegistrarUsuario_clubvinali",
            new
            {
                usuario.Nombre,
                usuario.ApellidoPaterno,
                usuario.ApellidoMaterno,
                usuario.Correo,
                usuario.TipoDocumento,
                usuario.NumeroDocumento,
                ContrasenaHash = hash  // 👈 Se guarda el hash en la BD
            },
            commandType: CommandType.StoredProcedure
        ).ConfigureAwait(false);
    }
    // Obtener usuario por correo
    public async Task<UsuarioVinali?> ObtenerPorCorreo(string correo)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<UsuarioVinali>(
            "clubvinali.sp_ObtenerUsuarioPorCorreo_vinali", // nombre correcto del SP
            new { Correo = correo },
            commandType: CommandType.StoredProcedure
        ).ConfigureAwait(false);
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