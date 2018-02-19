using System;

namespace Pyatnashki.API.Models
{
    /// <summary>
    /// Game statistics
    /// </summary>
    public class Statistics
    {
        public int TurnsNumber { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public DateTime StartTime { get; set; }

        public Statistics(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}