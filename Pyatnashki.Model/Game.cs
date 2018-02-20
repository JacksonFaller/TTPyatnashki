using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Pyatnashki.Model
{
    public class Game
    {
        [Required]
        public string Name { get; set; }

        public Game(string name)
        {
            Name = name;
        }
    }
}
