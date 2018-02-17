using System;

namespace Pyatnashki.API.Models
{
    public class Statistics
    {
        public int MovesNumber { get; set; }
        public int Score { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public DateTime StartTime { get; set; }

        public Statistics(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}