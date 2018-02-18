using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyatnashki.DataLayer.SQL.Tests
{
    class MainTest
    {
        public const string ConnectionString = 
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=TTGamesDB;Integrated Security=True";

        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static readonly Random Random = new Random();

        public static string GetRandomName(int length)
        {
            var stringChars = new char[length];
           
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[Random.Next(Chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
