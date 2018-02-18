using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki.Model
{
    public class Score
    {
        public Guid Id { get; set; }
        public string PlayerName { get; set; }
        public string GameName { get; set; }
        public TimeSpan? Time { get; set; }
        public int? TurnsNumber { get; set; }
        public int? Scores { get; set; }

        public Score(Guid id, string playerName, string gameName, 
            TimeSpan? time = null, int? turnsNumber = null, int? scores = null)
        {
            Id = id;
            PlayerName = playerName;
            GameName = gameName;
            Time = time;
            TurnsNumber = turnsNumber;
            Scores = scores;
        }

        public Score()
        {
            
        }
    }
}
