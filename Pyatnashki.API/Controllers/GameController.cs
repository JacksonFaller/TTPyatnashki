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
    /// <summary>
    /// Base game controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameController<T>  : ApiController where T : GameModel, new()
    {
        private readonly ScoresRepository _scoresRepository;
        private readonly string _gameName;

        public GameController(string connectionString, string gameName)
        {
            _scoresRepository = new ScoresRepository(connectionString);
            _gameName = gameName;
        }

        public virtual T NewGame()
        {
            T model = new T();
            model.StartNewGame();
            return model;
        }

        public virtual Score GetScore(Guid id)
        {
            return _scoresRepository.GetScore(id);
        }

        public Score SaveScore([FromBody]Player player, [FromBody]T model)
        {
            if (!model.IsGameCompleted)
                throw new InvalidOperationException("Game is not ended yet to save results!");

            var score = new Score(
                Guid.NewGuid(), player.Name, _gameName, model.GameStats.TimeSpent, model.GameStats.TurnsNumber);
            _scoresRepository.AddScore(score);

            return score;
        }
    }
}
