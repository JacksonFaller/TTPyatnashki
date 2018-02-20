using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pyatnashki.API.Models
{
    /// <summary>
    /// Base game model
    /// </summary>
    public abstract class GameModel
    {
        /// <summary>
        /// Indicates whether game completed or not
        /// </summary>
        public bool IsGameCompleted { get; protected set; }

        /// <summary>
        /// Game statistics such as score, time, turns number.
        /// </summary>
        [Required]
        public Statistics GameStats { get; set; }

        /// <summary>
        /// Checks if game is completed
        /// </summary>
        /// <returns>true if game is completed, else false</returns>
        public abstract bool IsGameCompletedCheck();

        /// <summary>
        /// Start a new game
        /// </summary>
        public abstract void StartNewGame();
    }
}