using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL
{
    public class GamesRepository : IGamesRepository
    {
        private readonly string _connectionString;

        public GamesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddGame(Game game)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Games values (@name)";
                    command.Parameters.AddWithValue("@name", game.Name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Game GetGame(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Games where name = @name";
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        return new Game
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                }
            }
        }
    }
}
