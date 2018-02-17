using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer
{
    public interface IPlayersRepository
    {
        void AddPlayer(Player player);
        Player GetPlayer(string name);
    }
}
