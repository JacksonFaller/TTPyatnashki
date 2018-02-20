using System;

namespace Pyatnashki.API.Models
{
    /// <summary>
    /// Game statistics
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Turns number made by player to complete the game
        /// </summary>
        public int TurnsNumber { get; set; }
        /// <summary>
        /// Amount of time spent to complete the game
        /// </summary>
        public TimeSpan TimeSpent { get; set; }
        /// <summary>
        /// Time when player started the game
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Initialize statistics
        /// </summary>
        /// <param name="startTime">time when the game started</param>
        public Statistics(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}