using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class CompraRepository
{
    private readonly string _connectionString;

    public CompraRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MegaBotikasConnection")
            ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
    }



    public async Task<(IEnumerable<Compra> Compras, int TotalCount)> BuscarCompras(
            string? nroOCompra,
            string? nroFactura,
            string? cdArticulo,
            string? nombreProducto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? estadoOC,
            int page = 1,
            int pageSize = 50)
    {
        using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@NroOCompra", nroOCompra);
        parameters.Add("@NroFactura", nroFactura);
        parameters.Add("@CdArticulo", cdArticulo);
        parameters.Add("@NombreProducto", nombreProducto);
        parameters.Add("@FechaInicio", fechaInicio);
        parameters.Add("@FechaFin", fechaFin);
        parameters.Add("@EstadoOC", estadoOC);
        parameters.Add("@Page", page);
        parameters.Add("@PageSize", pageSize);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var compras = await connection.QueryAsync<Compra>(
            "sp_buscar_compras_general",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 120
        );

        int total = parameters.Get<int>("@TotalCount");
        return (compras, total);
    }

}
