using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer
{
    public interface IScoresRepository
    {
        void AddScore(Score score);
        IEnumerable<Score> GetScores(Game game, Player player);
    }
}
