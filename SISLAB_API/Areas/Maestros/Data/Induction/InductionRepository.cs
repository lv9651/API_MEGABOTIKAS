using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class InductionRepository
{
    private readonly string? _connectionString;

    public InductionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }







    public async Task<IEnumerable<Induction>> GetAllInductionAsync()
    {   
        using (var connection = CreateConnection())
        {
            var query = "GetInductionVideos";
            return await connection.QueryAsync<Induction>(query, commandType: CommandType.StoredProcedure);
        }
    }



    public async Task SaveInductionAsync(Induction Induction)
    {
        using (var connection = CreateConnection())
        {
            var query = "InsertInductionVideo"; // Nombre del procedimiento almacenado
            await connection.ExecuteAsync(query, new
            {
                title = Induction.title,
                content = Induction.content,
                video_url = Induction.video_url,
                module = Induction.module

            }, commandType: CommandType.StoredProcedure);
        }


    }






    public async Task<bool> DeleteNewsByIdAsync(int id)
    {
        string videoUrl = null;

        // Primero, obtén la URL del video de la base de datos
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var selectCommand = new MySqlCommand("SELECT video_url FROM induction_videos WHERE Id = @Id", connection);
            selectCommand.Parameters.AddWithValue("@Id", id);

            using (var reader = await selectCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    videoUrl = reader.GetString(0);
                }
                else
                {
                    return false; // Video no encontrado
                }
            }
        }

        // Elimina el video del sistema de archivos
        if (!string.IsNullOrEmpty(videoUrl))
        {
            var videoPath = Path.Combine(@"\\192.168.154.12\fileserver\TI\Velasquez\Videos", videoUrl);
            if (File.Exists(videoPath))
            {
                File.Delete(videoPath);
            }
        }

        // Ahora, elimina el video de la base de datos
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var deleteCommand = new MySqlCommand("DeleteVideo", connection);
            deleteCommand.CommandType = CommandType.StoredProcedure;
            deleteCommand.Parameters.AddWithValue("@videoId", id);

            await deleteCommand.ExecuteNonQueryAsync();
        }

        return true;
    }




    public async Task AddCommentAsync(int videoId, CommentR comment)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var sql = "INSERT INTO comments (video_id, comment, user_id) VALUES (@VideoId, @CommentText, @UserId)";
            using (var cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@VideoId", videoId);
                cmd.Parameters.AddWithValue("@CommentText", comment.comment);
                cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }







    public async Task<IEnumerable<Comment>> GetCommentsByVideoIdAsync(int videoId)
    {
        var comments = new List<Comment>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
            SELECT comments.comment, users.username AS user_name, users.id UserId
            FROM comments
            JOIN users ON comments.user_id = users.id
            WHERE comments.video_id = @VideoId";

                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@VideoId", videoId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comments.Add(new Comment
                            {
                                comment = reader.GetString(0),
                                user_name = reader.GetString(1),
                                UserId = reader.GetInt32(2) // Asegúrate de que tu clase Comment tenga una propiedad para UserName
                            });
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            // Maneja excepciones específicas de MySQL
            Console.Error.WriteLine($"MySQL error: {ex.Message}");
            // Aquí podrías lanzar una excepción personalizada o devolver un resultado de error
        }
        catch (Exception ex)
        {
            // Maneja cualquier otra excepción
            Console.Error.WriteLine($"Error fetching comments: {ex.Message}");
            // Aquí podrías lanzar una excepción personalizada o devolver un resultado de error
        }

        return comments;
    }


    public async Task<IEnumerable<VideoProgress>> GetVideoProgressAsync(string userId, string module)
    {
        var videoProgressList = new List<VideoProgress>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetVideoProgress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("module", module);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var videoProgress = new VideoProgress
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Content = reader.GetString(2),
                                VideoUrl = reader.GetString(3),
                                Module = reader.GetString(4),
                                CreatedAt = reader.GetDateTime(5)
                            };

                            videoProgressList.Add(videoProgress);
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            // Manejar excepciones específicas de MySQL
            Console.Error.WriteLine($"MySQL Error: {ex.Message}");
            throw; // O manejar el error según tu lógica
        }
        catch (Exception ex)
        {
            // Manejar excepciones generales
            Console.Error.WriteLine($"Error: {ex.Message}");
            throw; // O manejar el error según tu lógica
        }

        return videoProgressList;
    }
}