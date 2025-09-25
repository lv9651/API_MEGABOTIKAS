using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

public class PuntajeRepositorio
{
    private readonly string _connectionString;

    public PuntajeRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServerQFPHARMA")
            ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<ClientePuntosResponse> ActualizarPuntos(string correo)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<ClientePuntosResponse>(
            "[clubqf].[sp_ActualizarPuntosClientePorCorreo]",
            new { correo },
            commandType: CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<HistorialCanje>> ObtenerHistorialAsync(int idCliente)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<HistorialCanje>(
            "clubqf.sp_HistorialCanjes",
            new { IdCliente = idCliente },
            commandType: CommandType.StoredProcedure
        );
    }


    public async Task<NivelCompletoResponse> ObtenerNivelPorPuntos(decimal puntos, int idCliente)
    {
        using var db = Connection;

        using var multi = await db.QueryMultipleAsync(
            "clubqf.sp_ObtenerNivelPorPuntos",
            new { Puntos = puntos, IdCliente = idCliente },
            commandType: CommandType.StoredProcedure
        );

        var nivelInfo = await multi.ReadFirstOrDefaultAsync<NivelClienteResponse>();
        if (nivelInfo == null) return null;

        var beneficios = (await multi.ReadAsync<BeneficioNivel>()).ToList();
        var productos = (await multi.ReadAsync<ProductoNivel>()).ToList();

        return new NivelCompletoResponse
        {
            NivelInfo = nivelInfo,
            Beneficios = beneficios,
            Productos = productos
        };
    }

    public async Task<(NivelClienteResponse Nivel, List<dynamic> Beneficios, List<dynamic> Productos)> ObtenerNivel(string correo)
    {
        using var db = Connection;
        using var multi = await db.QueryMultipleAsync(
            "[clubqf].[sp_ObtenerNivelCliente]",
            new { correo },
            commandType: CommandType.StoredProcedure);

        var nivel = await multi.ReadFirstOrDefaultAsync<NivelClienteResponse>();
        var beneficios = (await multi.ReadAsync()).ToList();
        var productos = (await multi.ReadAsync()).ToList();

        return (nivel, beneficios, productos);
    }

    public async Task<CanjeResponse> Canjear(CanjeRequest request)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<CanjeResponse>(
            "[clubqf].[sp_Canjear]",
            new
            {
                correo = request.Correo,
                tipo = request.Tipo,
                idReferencia = request.IdReferencia,
      
            },
            commandType: CommandType.StoredProcedure);
    }

}