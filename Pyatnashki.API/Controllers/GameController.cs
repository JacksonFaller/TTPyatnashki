using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pyatnashki.API.Models;
using Pyatnashki.DataLayer.SQL;
using Pyatnashki.Model;

namespace Pyatnashki.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Base game controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameController<T>  : ApiController where T : GameModel, new()
    {
        private readonly ScoresRepository _scoresRepository;
        private readonly string _gameName;
        
        /// <summary>
        /// Initialize game controller
        /// </summary>
        /// <param name="connectionString">database connection string</param>
        /// <param name="gameName">game name</param>
        public GameController(string connectionString, string gameName)
        {
            _scoresRepository = new ScoresRepository(connectionString);
            _gameName = gameName;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <returns>game model</returns>
        public virtual T NewGame()
        {
            Logger.Logger.Instance.Info($"Starting new {_gameName} game.");
            T model = new T();
            model.StartNewGame();
            return model;
        }

        /// <summary>
        /// Get score by id
        /// </summary>
        /// <param name="id">score id</param>
        /// <returns>score model</returns>
        public virtual Score GetScore(Guid id)
        {
            Logger.Logger.Instance.Info($"Getting score by id: {id}.");
            return _scoresRepository.GetScore(id);
        }

        /// <summary>
        /// Save new score
        /// </summary>
        /// <param name="model">game model</param>
        /// <param name="playerName">player name</param>
        /// <returns>score model</returns>
        public Score SaveScore([FromBody]T model, string playerName)
        {
            Logger.Logger.Instance.Info($"Saving score for player {playerName}.");

            if (playerName == null)
            {
                Logger.Logger.Instance.Error("Player's name is null.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    $"Parameter {nameof(playerName)} is required"));
            }

            Validate(model);
            if (!ModelState.IsValid)
            {
                Logger.Logger.Instance.Error(ModelState.GetErrors());
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            if (!model.IsGameCompleted)
            {
                Logger.Logger.Instance.Error("Game is not ended yet to save results.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Game is not ended yet to save results!"));
            }

            Guid id = Guid.NewGuid();
            var score = new Score(id, playerName, _gameName, model.GameStats.TimeSpent, model.GameStats.TurnsNumber);
            _scoresRepository.AddScore(score);

            Logger.Logger.Instance.Info($"New score with id: {id} has been saved.");
            return score;
        }
    }
}
