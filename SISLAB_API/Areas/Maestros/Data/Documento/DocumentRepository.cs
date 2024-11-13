using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;

public class DocumentRepository
{
    private readonly string? _connectionString;

    public DocumentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }


    public async Task InsertDocumentAsync(string descripcion, string mes, string anio, string archivoUrl)
        {
            string procedureName = "InsertDocument"; // Asegúrate de que el nombre del procedimiento almacenado sea correcto

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(procedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@descripcion", descripcion);
                    command.Parameters.AddWithValue("@mes", mes);
                    command.Parameters.AddWithValue("@anio", anio);
                    command.Parameters.AddWithValue("@archivo_url", archivoUrl);
                
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    public async Task InsertFirmaAsync(string dni, string document_file)
    {
        string procedureName = "RegisterFirma"; // Asegúrate de que el nombre del procedimiento almacenado sea correcto

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new MySqlCommand(procedureName, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_dni", dni);
        
                command.Parameters.AddWithValue("@p_document_file", document_file);

                await command.ExecuteNonQueryAsync();
            }
        }
    }









    public async Task InsertBenefAsync(string dni, string descripcion, string beneficio, string archivoUrl)
    {
        string procedureName = "InsertBenefic"; // Asegúrate de que el nombre del procedimiento almacenado sea correcto

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new MySqlCommand(procedureName, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dni", dni);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@beneficio", beneficio);
                command.Parameters.AddWithValue("@ruta_doc", archivoUrl);

                await command.ExecuteNonQueryAsync();
            }
        }
    }





    public async Task<bool> DeleteNewsByIdAsync(int ide)
    {
        string dni = null;
        string beneficio = null;
        string ruta_doc = null;


        // Primero, obtén la URL del video de la base de datos
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var selectCommand = new MySqlCommand("SELECT dni,beneficio,ruta_doc FROM benefic_empleado WHERE Id = @Ide", connection);
            selectCommand.Parameters.AddWithValue("@Ide", ide);

            using (var reader = await selectCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    dni = reader.GetString(0);
                    beneficio = reader.GetString(1);
                    ruta_doc = reader.GetString(2);
                }
                else
                {
                    return false; // Video no encontrado
                }
            }
        }

        // Elimina el video del sistema de archivos
        if (!string.IsNullOrEmpty(ruta_doc))
        {
            var videoPath = Path.Combine(@"\\PANDAFILE\Intranet\empleado", dni, beneficio, ruta_doc);
            if (File.Exists(videoPath))
            {
                File.Delete(videoPath);
            }
        }

        // Ahora, elimina el video de la base de datos
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var deleteCommand = new MySqlCommand("DeleteBenef", connection);
            deleteCommand.CommandType = CommandType.StoredProcedure;
            deleteCommand.Parameters.AddWithValue("@ide", ide);

            await deleteCommand.ExecuteNonQueryAsync();
        }

        return true;
    }



    public async Task<IEnumerable<BeneficioEmp>> GetBenefemple()
    {
        using (var connection = CreateConnection())
        {
            var query = "GetBenefemple";
            return await connection.QueryAsync<BeneficioEmp>(query, commandType: CommandType.StoredProcedure);
        }
    }















}
