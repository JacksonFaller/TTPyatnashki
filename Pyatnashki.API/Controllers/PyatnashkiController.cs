using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pyatnashki.API.Models;

namespace Pyatnashki.API.Controllers
{
    public class PyatnashkiController : ApiController
    {
        private const string ConnectionString =
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=TTGamesDB;Integrated Security=True";

        [HttpGet]
        [Route("api/games/pyatnashki/new")]
        public  PyatnashkiModel NewGame()
        {
            // May add size to route (api/games/pyatnashki/new/{size}"). I'm just gonna use 4 for now.
            PyatnashkiModel model = new PyatnashkiModel(GenerateRandomField(4), DateTime.UtcNow);
            return model;
        }

        [HttpGet]
        [Route("api/games/pyatnashki/move/{direction}")]
        public PyatnashkiModel MakeMove([FromBody]PyatnashkiModel model, Directions direction)
        {
            MoveCell(direction, model.Field, model.EmptyCell);
            model.GameStats.MovesNumber++;
            
            if (IsSolved(model.Field))
            {
                model.IsSolved = true;
                model.GameStats.TimeSpent = DateTime.UtcNow - model.GameStats.StartTime;
                // Inset formula to calculate score
                // Sql query to save game results

            }
            return model;
        }
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
