using Dapper;
using MySql.Data.MySqlClient;
using SISLAB_API.Areas.Maestros.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class AgendaRepository 
    {
        private readonly string? _connectionString;

        public AgendaRepository(IConfiguration  configuration)
        {
        _connectionString = configuration.GetConnectionString("SislabConnection");

    }

        public void AddAgendaItem(AgendaItem item)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Agenda (meeting_name, dni, date, time) VALUES (@MeetingName, @Dni, @Date, @Time)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MeetingName", item.MeetingName);
                    command.Parameters.AddWithValue("@Dni", item.Dni);
                    command.Parameters.AddWithValue("@Date", item.Date);
                    command.Parameters.AddWithValue("@Time", item.Time);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAgendaItem(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Agenda WHERE id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<AgendaItem> GetAgendaItems(DateTime date)
        {
            var items = new List<AgendaItem>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Agenda WHERE date = @Date";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new AgendaItem
                            {
                                Id = reader.GetInt32("id"),  
                                MeetingName = reader.GetString("meeting_name"),
                                Dni = reader.GetString("dni"),
                                Date = reader.GetDateTime("date").ToString("yyyy-MM-dd"),
                                Time = reader.GetTimeSpan("time"),
                            });
                        }
                    }
                }
            }
            return items;
        }
    }
