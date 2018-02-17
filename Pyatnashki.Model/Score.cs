using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki.Model
{
    public class Score
    {
        public Guid MatchId { get; set; }
        public string PlayerName { get; set; }
        public string GameName { get; set; }
        public TimeSpan Time { get; set; }
        public int TurnsNumber { get; set; }
        public int Scores { get; set; }
    }
}
