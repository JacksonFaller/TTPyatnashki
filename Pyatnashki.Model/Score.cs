using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki.Model
{
    public class Score
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string PlayerName { get; set; }
        [Required]
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
