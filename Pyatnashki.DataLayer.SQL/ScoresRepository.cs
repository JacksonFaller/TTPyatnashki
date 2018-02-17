using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL
{
    class ScoresRepository : IScoresRepository
    {
        private readonly string _connectionString;

        public ScoresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddScore(Score score)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = 
                        "insert into Scores values (@matchId, @playerName, @gameName, @time, @score, @turnsNumber)";
                    command.Parameters.AddWithValue("@matchId", score.MatchId);
                    command.Parameters.AddWithValue("@playerName", score.PlayerName);
                    command.Parameters.AddWithValue("@gameName", score.GameName);
                    command.Parameters.AddWithValue("@time", score.Time);
                    command.Parameters.AddWithValue("@score", score.Scores);
                    command.Parameters.AddWithValue("@turnsNumber", score.TurnsNumber);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator<Score> GetScores(Game game, Player player)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Scores where playerName = @playerName and gameName = @gameName";
                    command.Parameters.AddWithValue("@playerName", player.Name);
                    command.Parameters.AddWithValue("@gameName", game.Name);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Score
                            {
                                //Name = reader.GetString(reader.GetOrdinal("Name"))
                                GameName = game.Name,
                                PlayerName = player.Name,
                                Scores = reader.GetInt32(reader.GetOrdinal("Score")),
                                Time = TimeSpan.FromTicks(reader.GetInt64(reader.GetOrdinal("Time"))),
                                TurnsNumber = reader.GetInt32(reader.GetOrdinal("TurnsNumber")),
                                MatchId = reader.GetGuid(reader.GetOrdinal("MatchId"))
                            };
                        }
                    }
                }
            }
        }
    }
}
