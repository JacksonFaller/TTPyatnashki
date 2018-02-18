using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Pyatnashki.Model;
using static Pyatnashki.DataLayer.SQL.Tests.MainTest;

namespace Pyatnashki.DataLayer.SQL.Tests
{
    [TestClass]
    public class ScoresRepositoryTest
    {
        private readonly List<Guid> _scoresList = new List<Guid>(16);
        private readonly List<string> _playersList = new List<string>(16);
        private readonly List<string> _gamesList = new List<string>(16);

        [TestMethod]
        public void AddGetScoreTest()
        {
            PlayersRepository playersRepository = new PlayersRepository(ConnectionString);
            GamesRepository gamesRepository = new GamesRepository(ConnectionString);
            ScoresRepository scoresRepository = new ScoresRepository(ConnectionString);

            string playerName = GetRandomName(10);
            string gameName = GetRandomName(10);

            playersRepository.AddPlayer(new Player(playerName));
            _playersList.Add(playerName);

            gamesRepository.AddGame(new Game(gameName));
            _gamesList.Add(gameName);

            Guid scoreId = Guid.NewGuid();
            _scoresList.Add(scoreId);
            Score score = new Score(scoreId, playerName, gameName, scores: 5);
            scoresRepository.AddScore(score);
            
            Assert.AreEqual(scoreId, scoresRepository.GetScore(scoreId).Id);
        }

        [TestMethod]
        public void GetScoresTest()
        {
            PlayersRepository playersRepository = new PlayersRepository(ConnectionString);
            GamesRepository gamesRepository = new GamesRepository(ConnectionString);
            ScoresRepository scoresRepository = new ScoresRepository(ConnectionString);

            string playerName = GetRandomName(10);
            string gameName = GetRandomName(10);

            playersRepository.AddPlayer(new Player(playerName));
            _playersList.Add(playerName);

            gamesRepository.AddGame(new Game(gameName));
            _gamesList.Add(gameName);

            Guid scoreId1 = Guid.NewGuid(), scoreId2 = Guid.NewGuid();
            _scoresList.Add(scoreId1);
            _scoresList.Add(scoreId2);

            Score score = new Score(scoreId1, playerName, gameName, scores: 5);
            scoresRepository.AddScore(score);
            score = new Score(scoreId2, playerName, gameName, scores: 15);
            scoresRepository.AddScore(score);

            List<Score> scores = scoresRepository.GetScores(new Game(gameName), new Player(playerName)).ToList();
            if(scores.Find(x => x.Id == scoreId1) == null)
                Assert.Fail($"Expected score id is {scoreId1}");
            if (scores.Find(x => x.Id == scoreId2) == null)
                Assert.Fail($"Expected score id is {scoreId1}");
        }

        [TestCleanup]
        public void CleanUp()
        {
            var scoresRepository = new ScoresRepository(ConnectionString);
            var gamesRepository = new GamesRepository(ConnectionString);
            var playersRepository = new PlayersRepository(ConnectionString);

            foreach (Guid scoreId in _scoresList)
            {
                scoresRepository.DeleteScore(scoreId);
            }

            foreach (string gameName in _gamesList)
            {
                gamesRepository.DeleteGame(gameName);
            }

            foreach (string playerName in _playersList)
            {
                playersRepository.DeletePlayer(playerName);
            }
        }
    }
}
