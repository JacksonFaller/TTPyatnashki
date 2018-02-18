using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pyatnashki.Model;

namespace Pyatnashki.DataLayer.SQL.Tests
{
    [TestClass]
    public class PlayersRepositoryTests
    {
        private readonly List<string> _playersList = new List<string>(16);

        [TestMethod]
        public void AddGetPlayerTest()
        {
            PlayersRepository repository = new PlayersRepository(MainTest.ConnectionString);
            string name = MainTest.GetRandomName(10);
            repository.AddPlayer(new Player(name));
            _playersList.Add(name);

            Assert.AreEqual(name, repository.GetPlayer(name).Name);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if(_playersList.Count == 0) return;
            PlayersRepository repository = new PlayersRepository(MainTest.ConnectionString);

            foreach (string playerName in _playersList)
            {
                repository.DeletePlayer(playerName);
            }
        }
    }
}
