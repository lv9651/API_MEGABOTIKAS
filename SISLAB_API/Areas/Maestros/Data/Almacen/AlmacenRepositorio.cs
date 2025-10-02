using API_MEGABOTIKAS.Areas.Maestros.Models.Almacen;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_MEGABOTIKAS.Areas.Maestros.Data.Almacen
{
    public class AlmacenRepositorio
    {

        private readonly string _connectionString;

        public AlmacenRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MegaBotikasConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "La cadena de conexión no está configurada");
        }
        public async Task<IEnumerable<StockLocalDto>> obtenerStock_X_Local(int codArticulo)
        {
            using var conexion = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@codArticulo", codArticulo);

            return await conexion.QueryAsync<StockLocalDto>("stock.sp_obtenerStock_X_Local", parameters, commandType: CommandType.StoredProcedure);
        }


    }
}
