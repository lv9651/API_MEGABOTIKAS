using Dapper;
using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Data;
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

    // Registrar un usuario y devolver el Id generado
    public async Task<IEnumerable<Puntaje>> ObtenerVentasPuntosAsync(int idUsuario)
    {
          using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"EXEC [clubvinali].sp_ObtenerVentas_puntos @idUsuario";

            return await connection.QueryAsync<Puntaje>(query, new { idUsuario });
        }
    }

    // Insertar un nuevo historial de descuento
    public void InsertarHistorialDescuento(HistorialDescuento historialDescuento)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = @"
                EXEC clubvinali.InsertarHistorialDescuento
                    @idCliente,
                    @puntosTotal,
                    @puntosAntesDescuento,
                    @puntosDescuento,
                    @nivelAnterior,
                    @descuentoAplicado,
                    @nivelFinal;
            ";

            connection.Execute(query, new
            {
                historialDescuento.IdCliente,
                historialDescuento.PuntosTotal,
                historialDescuento.PuntosAntesDescuento,
                historialDescuento.PuntosDescuento,
                historialDescuento.NivelAnterior,
                historialDescuento.DescuentoAplicado,
                historialDescuento.NivelFinal
            });
        }
    }

    // Obtener el historial de descuentos de un cliente
    public IEnumerable<HistorialDescuento> ObtenerHistorialPorCliente(int idCliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM HistorialDescuentos WHERE idCliente = @idCliente";
            return connection.Query<HistorialDescuento>(query, new { idCliente }).ToList();
        }
    }
}