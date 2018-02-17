using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Pyatnashki.API.Models
{
    public class PyatnashkiModel
    {
        public Guid Id { get; set; }
        public int[,] Field { get; set; }
        public bool IsSolved { get; set; }
        public Statistics GameStats { get; set; }

        public Point EmptyCell { get; set; }

        public PyatnashkiModel(int [,] field) : this(field, DateTime.UtcNow)
        {
        }

        public PyatnashkiModel(int[,] field, DateTime startTime)
        {
            Id = new Guid();
            Field = field;
            EmptyCell = FindEmptyCell(field);
            GameStats = new Statistics(startTime);
        }

        private Point FindEmptyCell(int[,] field)
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if(field[i, j] == 0) return new Point(i, j);
                }
            }
            throw new ArgumentException("Field dosen't contain empty cell!", nameof(field));
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