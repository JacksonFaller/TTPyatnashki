using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL
{
    public class ScoresRepository : IScoresRepository
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
                    StringBuilder commandBuilder = new StringBuilder(
                        "insert into Scores(Id, PlayerName, GameName");

                    StringBuilder values = new StringBuilder("values (@id, @playerName, @gameName");

                    command.Parameters.AddWithValue("@id", score.Id);
                    command.Parameters.AddWithValue("@playerName", score.PlayerName);
                    command.Parameters.AddWithValue("@gameName", score.GameName);

                    if(score.Time == null && score.TurnsNumber == null && score.Scores == null)
                        throw new ArgumentException(
                            "At least one parameter of the score (Time, TurnsNumber, Scores) should be not null");

                    if (score.Time != null)
                    {
                        commandBuilder.Append(", Time");
                        values.Append(", @time");
                        command.Parameters.AddWithValue("@time", score.Time?.Ticks);
                    }
                    if (score.TurnsNumber != null)
                    {
                        commandBuilder.Append(", TurnsNumber");
                        values.Append(", @turnsNumber");
                        command.Parameters.AddWithValue("@turnsNumber", score.TurnsNumber);
                    }
                    if (score.Scores != null)
                    {
                        commandBuilder.Append(", Score");
                        values.Append(", @score");
                        command.Parameters.AddWithValue("@score", score.Scores);
                    }

                    commandBuilder.Append($"){values})");
                    command.CommandText = commandBuilder.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Score> GetScores(Game game, Player player)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Scores where PlayerName = @playerName and GameName = @gameName";
                    command.Parameters.AddWithValue("@playerName", player.Name);
                    command.Parameters.AddWithValue("@gameName", game.Name);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? score = null, turnsNumber = null;
                            TimeSpan? time = null;

                            if (!reader.IsDBNull(reader.GetOrdinal("Score")))
                                score = reader.GetInt32(reader.GetOrdinal("Score"));
                            if (!reader.IsDBNull(reader.GetOrdinal("Time")))
                                time = TimeSpan.FromTicks(reader.GetInt64(reader.GetOrdinal("Time")));
                            if (!reader.IsDBNull(reader.GetOrdinal("TurnsNumber")))
                                turnsNumber = reader.GetInt32(reader.GetOrdinal("TurnsNumber"));

                            yield return new Score
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                PlayerName = player.Name,
                                GameName = game.Name,
                                Time = time,
                                TurnsNumber = turnsNumber,
                                Scores = score
                            };
                        }
                    }
                }
            }
        }

        public Score GetScore(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from Scores where id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw  new ArgumentException($"Результат с id: {id} не найден");

                        int? score = null, turnsNumber = null;
                        TimeSpan? time = null;

                        if (!reader.IsDBNull(reader.GetOrdinal("Score")))
                            score = reader.GetInt32(reader.GetOrdinal("Score"));
                        if (!reader.IsDBNull(reader.GetOrdinal("Time")))
                            time = TimeSpan.FromTicks(reader.GetInt64(reader.GetOrdinal("Time")));
                        if (!reader.IsDBNull(reader.GetOrdinal("TurnsNumber")))
                            turnsNumber = reader.GetInt32(reader.GetOrdinal("TurnsNumber"));

                        return new Score
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            PlayerName = reader.GetString(reader.GetOrdinal("PlayerName")),
                            GameName = reader.GetString(reader.GetOrdinal("GameName")),
                            Time = time,
                            TurnsNumber = turnsNumber,
                            Scores = score
                        };
                    }
                }
            }
        }

        public bool DeleteScore(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Scores output deleted.id where id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }
    }
}
