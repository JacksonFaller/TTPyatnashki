using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki.Model
{
    public class Player
    {
        [Required]
        public string Name { get; set; }

        public Player(string name)
        {
            Name = name;
        }
    }
}
