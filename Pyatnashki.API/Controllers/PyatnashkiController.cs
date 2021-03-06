﻿using System;
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
    /// <summary>
    /// Operations with game Pyatnashki
    /// </summary>
    [ExceptionHandling]
    public class PyatnashkiController : GameController<PyatnashkiModel>
    {
        private const string ConnectionString =
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=TTGamesDB;Integrated Security=True";

        private const string GameName = "Pyatnashki";

        /// <summary>
        /// Initialize scores repository
        /// </summary>
        public PyatnashkiController() : base(ConnectionString, GameName)
        {
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <returns>new game model</returns>
        [HttpGet]
        [Route("api/games/pyatnashki/new")]
        public new PyatnashkiModel NewGame()
        {
            return base.NewGame();
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
            Logger.Logger.Instance.Info($"Making move in direction: {direction.ToString()}.");

            Validate(model);
            if (!ModelState.IsValid)
            {
                Logger.Logger.Instance.Error(ModelState.GetErrors());
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            if (model.IsGameCompleted)
            {
                Logger.Logger.Instance.Error("Game is completed, no moves allowed.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Game is ended. You can not make moves anymore. Call SaveScore to save results."));
            }

            MoveCell(direction, model.Field, model.EmptyCell);
            model.GameStats.TurnsNumber++;
            
            // Check field state
            if (model.IsGameCompletedCheck())
                // Calculate game time
                model.GameStats.TimeSpent = DateTime.UtcNow - model.GameStats.StartTime;

            return model;
        }

        /// <summary>
        /// Save game results
        /// </summary>
        /// <param name="playerName">player name</param>
        /// <param name="model">game model</param>
        /// <returns>score model</returns>
        [HttpPost]
        [Route("api/games/pyatnashki/save/{playerName}")]
        public new Score SaveScore([FromBody]PyatnashkiModel model, string playerName)
        {
            return base.SaveScore(model, playerName);
        }

        /// <summary>
        /// Get score by id
        /// </summary>
        /// <param name="id">result id</param>
        /// <returns>score model</returns>
        [HttpGet]
        [Route("api/games/pyatnashki/results/{id}")]
        public new Score GetScore(Guid id)
        {
            return base.GetScore(id);
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
