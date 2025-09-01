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

    // Registrar un usuario y devolver el Id generado
    public async Task<List<AcumulacionPuntos>> ObtenerVentasPuntos(int idUsuario)
    {
        var resultados = new List<AcumulacionPuntos>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("clubvinali.sp_ObtenerVentas_puntos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultados.Add(new AcumulacionPuntos
                        {
                            FechaMovimiento = reader.GetDateTime(0),
                            TipoMovimiento = reader.GetString(1),
                            Puntos = reader.GetDecimal(2),
                            Descripcion = reader.GetString(3),
                            Movimiento = reader.GetString(4),
                            Serie = reader.IsDBNull(5) ? null : reader.GetString(5),
                            NumDocumento = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Sucursal = reader.IsDBNull(7) ? null : reader.GetString(7)
                        });
                    }
                }
            }
        }

        return resultados;
    }

    public async Task<List<ProductoCanjeable>> ObtenerProductosCanjeables(int? idUsuario)
    {
        var resultados = new List<ProductoCanjeable>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("clubvinali.sp_ObtenerProductosCanjeables", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (idUsuario.HasValue)
                    command.Parameters.AddWithValue("@idUsuario", idUsuario.Value);
                else
                    command.Parameters.AddWithValue("@idUsuario", DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultados.Add(new ProductoCanjeable
                        {
                            Codigo = reader.GetInt32(0),
                            Servicio = reader.GetString(1),
                            Puntaje = reader.GetDecimal(2),
                            FechaCreacion = reader.GetDateTime(3),
                            Activo = reader.GetBoolean(4),
                            PuedeCanjear = Convert.ToBoolean(reader.GetInt32(5)),
                            PuntosDisponibles = reader.GetDecimal(6)
                        });
                    }
                }
            }
        }

        return resultados;
    }

    public async Task<ResultadoCanje> CanjearProducto(int idUsuario, int codigoProducto,string tipomovimiento)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("clubvinali.sp_CanjearProducto", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@codigoProducto", codigoProducto);
                command.Parameters.AddWithValue("@tipomovimiento", tipomovimiento);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ResultadoCanje
                        {
                            Mensaje = reader.GetString(0),
                            PuntosUtilizados = reader.GetDecimal(1),
                            NuevoSaldo = reader.GetDecimal(2),
                            Descripcion = reader.GetString(3)
                        };
                    }
                }
            }
        }

        throw new Exception("No se pudo procesar el canje");
    }

    public async Task<decimal> ObtenerSaldoPuntos(int idUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("clubvinali.sp_ObtenerSaldoPuntos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToDecimal(result);
            }
        }
    }

    public async Task<List<HistorialCompleto>> ObtenerHistorialCompleto(int idUsuario)
    {
        var resultados = new List<HistorialCompleto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("clubvinali.sp_ObtenerHistorialCompleto", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultados.Add(new HistorialCompleto
                        {
                            FechaMovimiento = reader.GetDateTime(0),
                            TipoMovimiento = reader.GetString(1),
                            Puntos = reader.GetDecimal(2),
                            Descripcion = reader.GetString(3),
                            Movimiento = reader.GetString(4),
                            Serie = reader.IsDBNull(5) ? null : reader.GetString(5),
                            NumDocumento = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Sucursal = reader.IsDBNull(7) ? null : reader.GetString(7),
                            ProductoCanjeado = reader.IsDBNull(8) ? null : reader.GetString(8)
                        });
                    }
                }
            }
        }

        return resultados;
    }

    public async Task<IEnumerable<ProductosPuntos>> ObtenerTodosServicios()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "EXEC [clubvinali].[sp_ObtenerServicios]";
                var servicios = await connection.QueryAsync<ProductosPuntos>(query);
                return servicios;
            }
        }
        catch (SqlException sqlEx)
        {
            throw new Exception($"Error de base de datos al obtener servicios: {sqlEx.Message}", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener servicios desde la base de datos", ex);
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