using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class ProductoRepositorio
{
    private readonly string _connectionString;

    public ProductoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MegaBotikasConnection")
            ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
    }

    // Método para obtener productos con paginación y filtrado
    public async Task<(IEnumerable<Producto> Productos, int Total)> ObtenerProductosAsync(
        int pagina, int tamanoPagina, string estado, string categoria, string laboratorio,string Nombre)
    {
        using var connection = new SqlConnection(_connectionString);

        string filtro = "";
        if (!string.IsNullOrEmpty(estado))
            filtro += " AND estado = @estado";
        if (!string.IsNullOrEmpty(categoria))
            filtro += " AND categoria = @categoria";
        if (!string.IsNullOrEmpty(laboratorio))
            filtro += " AND laboratorio = @laboratorio";
        if (!string.IsNullOrEmpty(Nombre))
            filtro += " AND laboratorio = @laboratorio";

        string sql = $@"
            SELECT *
            FROM (
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY Nombre) AS RowNum,
                    *
                FROM General_quiebre_Megabotikas
                WHERE 1=1 {filtro}
            ) AS Paged
            WHERE RowNum BETWEEN ((@pagina - 1) * @tamanoPagina + 1) AND (@pagina * @tamanoPagina);

            SELECT COUNT(*) 
            FROM General_quiebre_Megabotikas
            WHERE 1=1 {filtro};
        ";

        using var multi = await connection.QueryMultipleAsync(sql, new { pagina, tamanoPagina, estado, categoria, laboratorio,Nombre });
        var productos = await multi.ReadAsync<Producto>();
        var total = await multi.ReadSingleAsync<int>();

        return (productos, total);
    }


    public async Task<IEnumerable<RotacionLocalDto>> ObtenerRotacionPorLocal(string codigoProducto)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@CodigoProducto", codigoProducto, DbType.String);

        var result = await connection.QueryAsync<RotacionLocalDto>(
            "dbo.sp_RotacionPorLocal_Producto",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result;
    }
    // 🔹 Para llenar los combos de filtros
    public async Task<object> ObtenerFiltrosAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = @"
            SELECT DISTINCT estado FROM General_quiebre_Megabotikas WHERE estado IS NOT NULL;
            SELECT DISTINCT categoria FROM General_quiebre_Megabotikas WHERE categoria IS NOT NULL;
            SELECT DISTINCT laboratorio FROM General_quiebre_Megabotikas WHERE laboratorio IS NOT NULL;
            SELECT DISTINCT Nombre FROM General_quiebre_Megabotikas WHERE laboratorio IS NOT NULL;
        ";

        using var multi = await connection.QueryMultipleAsync(sql);
        var estados = (await multi.ReadAsync<string>()).ToList();
        var categorias = (await multi.ReadAsync<string>()).ToList();
        var laboratorios = (await multi.ReadAsync<string>()).ToList();
        var Nombres = (await multi.ReadAsync<string>()).ToList();

        return new { estados, categorias, laboratorios,Nombres };
    }
}