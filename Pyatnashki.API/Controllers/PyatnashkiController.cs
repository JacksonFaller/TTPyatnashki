using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using Pyatnashki.API.Models;
using Pyatnashki.DataLayer;
using Pyatnashki.DataLayer.SQL;
using Pyatnashki.Model;

namespace Pyatnashki.API.Controllers
{
    [ExceptionHandling]
    public class PyatnashkiController : ApiController
    {
        private const string ConnectionString =
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=TTGamesDB;Integrated Security=True";

        private readonly IGamesRepository _gamesRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly ScoresRepository _scoresRepository;

        private const string GameName = "Pyatnashki";

        /// <summary>
        /// Initialize repositories
        /// </summary>
        public PyatnashkiController()
        {
            _gamesRepository = new GamesRepository(ConnectionString);
            _playersRepository = new PlayersRepository(ConnectionString);
            _scoresRepository = new ScoresRepository(ConnectionString);
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <returns>new game model</returns>
        [HttpGet]
        [Route("api/games/pyatnashki/new")]
        public  PyatnashkiModel NewGame()
        {
            // May add size to route (api/games/pyatnashki/new/{size}"). I'm just gonna use 4 for now.
            PyatnashkiModel model = new PyatnashkiModel(GenerateRandomField(4), DateTime.UtcNow);
            return model;
        }

        /// <summary>
        /// Move element on the game field
        /// </summary>
        /// <param name="model">game model</param>
        /// <param name="direction">move direction</param>
        /// <returns>updated model</returns>
        [HttpGet]
        [Route("api/games/pyatnashki/move/{direction}")]
        public PyatnashkiModel MakeMove([FromBody]PyatnashkiModel model, Directions direction)
        {
            if (model.IsSolved)
                throw new InvalidOperationException(
                    "Game is ended. You can not make moves anymore. Call SaveScore to save results.");
            MoveCell(direction, model.Field, model.EmptyCell);
            model.GameStats.MovesNumber++;
            
            if (IsSolved(model.Field))
            {
                model.IsSolved = true;
                model.GameStats.TimeSpent = DateTime.UtcNow - model.GameStats.StartTime;
                // Inset formula to calculate score
            }
            return model;
        }

        /// <summary>
        /// Save game results
        /// </summary>
        /// <param name="player">player info</param>
        /// <param name="model">game model</param>
        /// <returns>score model</returns>
        [HttpPost]
        [Route("api/games/pyatnashki/save/")]
        public Score SaveScore([FromBody]Player player, [FromBody]PyatnashkiModel model)
        {
            if(_playersRepository.GetPlayer(player.Name) == null)
                _playersRepository.AddPlayer(player);

            if(_gamesRepository.GetGame(GameName) == null)
                _gamesRepository.AddGame(new Game(GameName));

            if (!model.IsSolved)
                throw new InvalidOperationException("Game is not ended yet to save results!");

            var score = new Score(
                Guid.NewGuid(), player.Name, GameName, model.GameStats.TimeSpent, model.GameStats.MovesNumber);
            _scoresRepository.AddScore(score);

            return score;
        }

        /// <summary>
        /// Get score by id
        /// </summary>
        /// <param name="id">result id</param>
        /// <returns>score model</returns>
        [HttpGet]
        [Route("api/games/pyatnashki/results/{id}")]
        public Score GetScore(Guid id)
        {
            return _scoresRepository.GetScore(id);
        }

        /// <summary>
        /// Generate random game field
        /// </summary>
        /// <param name="size">field size</param>
        /// <returns>square game field with randomized tile positions</returns>
        private int[,] GenerateRandomField(int size)
        {
            List<int> buffer = new List<int>(size * size);
            for (int i = 0; i < size * size; i++)
            {
                buffer.Add(i);
            }
            Random random = new Random();
            int[,] field = new int[size, size];
            int index;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    index = random.Next(buffer.Count);
                    field[i, j] = buffer[index];
                    buffer.RemoveAt(index);
                }
            }
            return field;
        }

        /// <summary>
        /// Check if field is in solved state
        /// </summary>
        /// <param name="field">square game field</param>
        /// <returns>true if solved, else false</returns>
        private bool IsSolved(int[,] field)
        {
            int value = 1;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    //if ((i + 1) * (j + 1) == field.Field.Length) break;
                    if (field[i, j] != value % field.Length) return false;
                    value++;
                }
            }
            return true;
        }

        /// <summary>
        /// Moves empty cell to certain direction
        /// </summary>
        /// <param name="direction">direction to move epmty cell</param>
        /// <param name="field">square game field</param>
        /// <param name="emptyCell">empty cell position</param>
        private void MoveCell(Directions direction, int[,] field, Point emptyCell)
        {
            switch (direction)
            {
                case Directions.Up:
                    if (emptyCell.x == field.GetLength(0))
                        throw new InvalidOperationException("You can't move Up from this position!");
                    field[emptyCell.x, emptyCell.y] = field[emptyCell.x + 1, emptyCell.y];
                    field[emptyCell.x + 1, emptyCell.y] = 0;
                    emptyCell.x++;
                    break;
                case Directions.Down:
                    if (emptyCell.x == 0)
                        throw new InvalidOperationException("You can't move Down from this position!");
                    field[emptyCell.x, emptyCell.y] = field[emptyCell.x - 1, emptyCell.y];
                    field[emptyCell.x - 1, emptyCell.y] = 0;
                    emptyCell.x--;
                    break;
                case Directions.Left:
                    if (emptyCell.y == 0)
                        throw new InvalidOperationException("You can't move Left from this position!");
                    field[emptyCell.x, emptyCell.y] = field[emptyCell.x, emptyCell.y - 1];
                    field[emptyCell.x, emptyCell.y - 1] = 0;
                    emptyCell.x--;
                    break;
                case Directions.Right:
                    if (emptyCell.y == field.GetLength(1))
                        throw new InvalidOperationException("You can't move Right from this position!");
                    field[emptyCell.x, emptyCell.y] = field[emptyCell.x, emptyCell.y + 1];
                    field[emptyCell.x, emptyCell.y + 1] = 0;
                    emptyCell.x++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Incorrect move direction");
            }
        }
    }
}
