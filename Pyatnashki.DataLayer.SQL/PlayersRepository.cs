using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL
{
    class PlayersRepository : IPlayersRepository
    {
        private readonly string _connectionString;

        public PlayersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddPlayer(Player player)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Players values (@name)";
                    command.Parameters.AddWithValue("@name", player.Name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Player GetPlayer(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Players where name = @name";
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        return new Player
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                }
            }
        }
    }
}
