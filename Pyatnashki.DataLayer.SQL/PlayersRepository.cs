using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL
{
    public class PlayersRepository : IPlayersRepository
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
                    command.CommandText = "select * from Players where Name = @name";
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                            //throw new ArgumentException($"Пользователь с именем: {name} не найден");
                        return new Player(reader.GetString(reader.GetOrdinal("Name")));
                    }
                }
            }
        }

        public bool DeletePlayer(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Players output deleted.Name where Name = @name";
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }
    }
}
