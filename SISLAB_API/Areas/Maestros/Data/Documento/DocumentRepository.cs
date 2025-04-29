using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    public async Task<bool> UpdateDocumentRouteAsync(int id, string rutaDoc)
    {
        string procedureName = "Firma_documento"; // Asegúrate de que este nombre sea correcto

        // Usar MySqlConnection para abrir la conexión
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                // Abrir la conexión
                await connection.OpenAsync();

                // Crear un comando para ejecutar el procedimiento almacenado
                using (var command = new MySqlCommand(procedureName, connection))
                {
                    // Establecer que el comando es un procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros requeridos
                    command.Parameters.AddWithValue("@ide", id);
                    command.Parameters.AddWithValue("@ruta_docc", rutaDoc);

                    // Ejecutar el procedimiento y obtener el número de filas afectadas
                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    // Retornar true si se actualizó al menos una fila
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones (por ejemplo, problemas de conexión, procedimientos no encontrados)
                Console.WriteLine($"Error al ejecutar el procedimiento: {ex.Message}");
                return false;
            }
        }
    }



    public async Task<bool> Vista_documentoByIdAsync(int id)
    {
        string procedureName = "Vista_documento"; // Asegúrate de que este nombre sea correcto

        // Usar MySqlConnection para abrir la conexión
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                // Abrir la conexión
                await connection.OpenAsync();

                // Crear un comando para ejecutar el procedimiento almacenado
                using (var command = new MySqlCommand(procedureName, connection))
                {
                    // Establecer que el comando es un procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros requeridos
                    command.Parameters.AddWithValue("@ide", id);
                

                    // Ejecutar el procedimiento y obtener el número de filas afectadas
                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    // Retornar true si se actualizó al menos una fila
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones (por ejemplo, problemas de conexión, procedimientos no encontrados)
                Console.WriteLine($"Error al ejecutar el procedimiento: {ex.Message}");
                return false;
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
