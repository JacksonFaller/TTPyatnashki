using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL.Tests
{
    /// <summary>
    /// Summary description for GamesRepositoryTest
    /// </summary>
    [TestClass]
    public class GamesRepositoryTest
    {
        private readonly List<string> _gamesList = new List<string>(16);

        [TestMethod]
        public void AddGetGameTest()
        {
            GamesRepository repository = new GamesRepository(MainTest.ConnectionString);
            string name = MainTest.GetRandomName(10);
            repository.AddGame(new Game(name));
            _gamesList.Add(name);

            Assert.AreEqual(name, repository.GetGame(name).Name);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (_gamesList.Count == 0) return;
            GamesRepository repository = new GamesRepository(MainTest.ConnectionString);

            foreach (string gameName in _gamesList)
            {
                repository.DeleteGame(gameName);
            }
        }
    }
}
