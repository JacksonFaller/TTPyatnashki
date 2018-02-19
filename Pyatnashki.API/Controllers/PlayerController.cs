using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pyatnashki.DataLayer;
using Pyatnashki.DataLayer.SQL;
using Pyatnashki.Model;

namespace Pyatnashki.API.Controllers
{
    /// <summary>
    /// Operations with players
    /// </summary>
    [ExceptionHandling]
    public class PlayerController : ApiController
    {
        private const string ConnectionString =
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=TTGamesDB;Integrated Security=True";
        private readonly IPlayersRepository _playersRepository;

        /// <summary>
        /// Initialize player repository
        /// </summary>
        public PlayerController()
        {
            _playersRepository = new PlayersRepository(ConnectionString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/players/{name}")]
        public Player NewPlayer(string name)
        {
            Player player = new Player(name);
            _playersRepository.AddPlayer(player);
           
            return player;
        }
    }
}
