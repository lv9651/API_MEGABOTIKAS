using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationRepository
{
    private readonly string? _connectionString;

    public NotificationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SislabConnection");
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        string insertNotificationQuery = "INSERT INTO notifications (user_id, message) VALUES (@UserId, @Message)";

        using (var connection = CreateConnection()) // Usar MySqlConnection
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(insertNotificationQuery, new
            {
                UserId = notification.UserId,
                Message = notification.Message
            });
        }
    }

    public async Task<IEnumerable<NotificationMessage>> GetNotificationsByIdAsync(string id)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "SELECT id,message FROM notifications WHERE user_id = @Id and is_read=0"; // Suponiendo que filtramos por UserId
            return await connection.QueryAsync<NotificationMessage>(query, new { Id = id });
        }
    }

    // Puedes añadir más métodos aquí, como actualizar o eliminar notificaciones
    public async Task<bool> MarkNotificationsAsReadAsync(string userId, List<int> notificationIds)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "UPDATE notifications SET is_read = TRUE WHERE id IN (@Ids) AND user_id = @UserId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Ids", string.Join(",", notificationIds));

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0;
            }
        }
    }
}
