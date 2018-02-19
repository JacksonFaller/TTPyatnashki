using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Pyatnashki.API.Models
{
    /// <summary>
    /// Pyatnashki game model
    /// </summary>
    public class PyatnashkiModel : GameModel
    {
        public int[,] Field { get; set; }

        public Point EmptyCell { get; set; }

        public PyatnashkiModel() : this(4)
        {
        }
        public PyatnashkiModel(int fieldSize) : this(fieldSize, DateTime.UtcNow)
        {
        }
        public PyatnashkiModel(int fieldSize, DateTime startTime)
        {
            Field = new int[fieldSize, fieldSize];
            GameStats = new Statistics(startTime);
        }

        public Point FindEmptyCell()
        {
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if(Field[i, j] == 0) return new Point(i, j);
                }
            }
            throw new ArgumentException("Field dosen't contain empty cell!", nameof(Field));
        }

        /// <summary>
        /// Checks if game is completed (field is solved)
        /// </summary>
        /// <returns>true if game is completed, else false</returns>
        public override bool IsGameCompletedCheck()
        {
            int value = 1;
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if (Field[i, j] != value % Field.Length) return false;
                    value++;
                }
            }
            IsGameCompleted = true;
            return true;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        public override void StartNewGame()
        {
            int size = Field.GetLength(0);

            List<int> buffer = new List<int>(size * size);
            for (int i = 0; i < size * size; i++)
            {
                buffer.Add(i);
            }
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var index = random.Next(buffer.Count);
                    Field[i, j] = buffer[index];
                    buffer.RemoveAt(index);
                }
            }
            EmptyCell = FindEmptyCell();
        }
    }

    public enum Directions
    {
        Up,
        Down,
        Left, 
        Right
    }

    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}