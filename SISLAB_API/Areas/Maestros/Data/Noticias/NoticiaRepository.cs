using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class NoticiaRepository
{
    private readonly string? _connectionString;

    public NoticiaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }



    public async Task InsertNewsAsync(InsertNoticia noticia)
    {
        using (var connection = CreateConnection())
        {
            var query = "InsertNews"; // Nombre del procedimiento almacenado
            await connection.ExecuteAsync(query, new
            {
                title = noticia.Title,
                content = noticia.content,
                image_url = noticia.image_url
            }, commandType: CommandType.StoredProcedure);
        }




    }








    public async Task<IEnumerable<Noticias>> GetAllNewsAsync()
    {   
        using (var connection = CreateConnection())
        {
            var query = "GetAllNews";
            return await connection.QueryAsync<Noticias>(query, commandType: CommandType.StoredProcedure);
        }
    }





    public async Task<bool> DeleteNewsByIdAsync(int id)
    {
        string imageUrl = null;

        // Primero, obtén la URL de la imagen de la noticia
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var selectCommand = new MySqlCommand("SELECT Image_url FROM news WHERE Id = @Id", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);

            using (var reader = await selectCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    imageUrl = reader.GetString(0);
                }
                else
                {
                    return false; // Noticia no encontrada
                }
            }
        }

        // Elimina la imagen del sistema de archivos
        if (!string.IsNullOrEmpty(imageUrl))
        {
            var imagePath = Path.Combine(@"\\PANDAFILE\Intranet\Img", imageUrl);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        // Ahora, elimina la noticia de la base de datos
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var deleteCommand = new MySqlCommand("DELETE FROM news WHERE Id = @Id", connection);
            deleteCommand.Parameters.AddWithValue("@Id", id);
            await deleteCommand.ExecuteNonQueryAsync();
        }

        return true;
    }









}