using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VideoReportApi.Repositories
{
    public class VideoReportRepository
    {
        private readonly string _connectionString;

        public VideoReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SislabConnection");
        }

        // Crear conexión con la base de datos
        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        // Método para obtener reportes filtrados usando el procedimiento almacenado de manera asincrónica
        public async Task<IEnumerable<ReporteVideo>> GetFilteredReportsAsync(string dni, string nombre, string videoStatus)
        {
            using (var dbConnection = CreateConnection())
            {
                // Abrir la conexión asincrónicamente
                await dbConnection.OpenAsync();

                // Ejecutar el procedimiento almacenado de forma asincrónica
                var query = "CALL Reporte_videos(@Dni, @Nombre, @VideoStatus)";
                var parameters = new { Dni = dni, Nombre = nombre, VideoStatus = videoStatus };

                // Ejecutar el procedimiento almacenado y mapear los resultados a VideoReport
                var reports = await dbConnection.QueryAsync<ReporteVideo>(query, parameters);
                return reports.ToList(); // Convertir el resultado en lista
            }
        }
    }
}