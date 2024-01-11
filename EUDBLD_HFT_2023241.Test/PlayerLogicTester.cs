using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static EUDBLD_HFT_2023241.Logic.PlayerLogic;

namespace EUDBLD_HFT_2023241.Test
{
    public class PlayerLogicTester : MyTestClass
    {
        PlayerLogic logic;
        Mock<IRepository<Player>> mockPlayerRepository;
        Mock<IRepository<PlayerChampionship>> mockPlayerChampionshipRepository;
        Mock<IRepository<Championship>> mockChampionshipRepository;

        [SetUp]
        public void Init()
        {
            base.SetAllDataToDefault();

            // MOQ Stuff
            mockPlayerRepository = new Mock<IRepository<Player>>();
            mockPlayerChampionshipRepository = new Mock<IRepository<PlayerChampionship>>();
            mockChampionshipRepository = new Mock<IRepository<Championship>>();

            // ReadAll
            mockPlayerRepository.Setup(p => p.ReadAll()).Returns(AllPlayers.AsQueryable);
            mockPlayerChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllPlayerChampionships.AsQueryable);
            mockChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllChampionships.AsQueryable);

            // Read
            for (int i = 0; i < AllPlayers.Count(); i++)
                mockPlayerRepository.Setup(p => p.Read(i + 1)).Returns(AllPlayers[i]);
            mockPlayerRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            mockChampionshipRepository.Setup(p => p.Read(It.IsAny<int>())).Returns(TestChampionship);
            // Delete
            mockPlayerRepository.Setup(p => p.Delete(444)).Throws<ArgumentNullException>();

            logic = new PlayerLogic(mockPlayerRepository.Object, mockPlayerChampionshipRepository.Object, mockChampionshipRepository.Object);
        }

        #region CRUD TESTS
        // CRUD TESTS
        [Test]
        public void CreatePlayerTest()
        {
            Player newPlayer = new Player() { Name = "New Player" };

            // ACT
            logic.Create(newPlayer);

            // ASSERT
            mockPlayerRepository.Verify(
                p => p.Create(newPlayer),
                Times.Once);
        }

        [Test]
        public void CreatePlayerUnsuccessfullyTest()
        {
            Player newPlayer = new Player() { Name = "Play" };

            Assert.That(() => logic.Create(newPlayer), Throws.ArgumentException);
        }

        [TestCase(1, "PlayerA")]
        [TestCase(2, "PlayerB")]
        [TestCase(3, "PlayerC")]
        [TestCase(4, "PlayerD")]
        public void ReadPlayer(int id, string expected)
        {
            Assert.That(() => logic.Read(id).Name, Is.EqualTo(expected));
        }

        [Test]
        public void ReadPlayerUnsuccessfullyTest()
        {
            Assert.That(() => logic.Read(444).Name, Throws.ArgumentNullException);
        }

        [Test]
        public void UpdatePlayerTest()
        {
            TestPlayer.Name = "New Name";
            logic.Update(TestPlayer);

            mockPlayerRepository.Verify(
                p => p.Update(TestPlayer),
                Times.Once);
        }

        [Test]
        public void UpdatePlayerUnsuccessfullyTest()
        {
            TestPlayer.Name = "New";

            Assert.That(() => logic.Update(TestPlayer), Throws.ArgumentException);
        }

        [Test]
        public void DeletePlayerTest()
        {
            logic.Delete(TestPlayer.Id);

            mockPlayerRepository.Verify(
                p => p.Delete(TestPlayer.Id),
                Times.Once);
        }

        [Test]
        public void DeletePlayerUnsuccessfullyTest()
        {
            Assert.That(() => logic.Delete(444), Throws.ArgumentNullException);
        }
        #endregion

        #region NON CRUD TESTS
        // NON CRUD TEST

        [Test]
        public void PlayersRankingMoneyTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            var result = logic.PlayersRankingMoney(TestPlayer.Id, time);

            // ASSERT
            Assert.That(result, Is.EqualTo(100000));
        }

        [Test]
        public void GetPlayersPrizeForChampionshipTest()
        {
            TestChampionship.Attenders = AllPlayers;
            TestChampionship.Prizes = new List<Prizes>()
            {
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 1, Price = 1000 },
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 2, Price = 500 },
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 3, Price = 200 },
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 4, Price = 100 }
            };

            Assert.That(() => logic.GetPlayersPrizeForChampionship(AllPlayers[0].Id, TestChampionship.Id), Is.EqualTo(100));
            Assert.That(() => logic.GetPlayersPrizeForChampionship(AllPlayers[2].Id, TestChampionship.Id), Is.EqualTo(500));
            Assert.That(() => logic.GetPlayersPrizeForChampionship(AllPlayers[3].Id, TestChampionship.Id), Is.EqualTo(1000));
        }

        [Test]
        public void GetPlayersPrizeForChampionshipUnsuccessfullyTest()
        {
            TestChampionship.Attenders = AllPlayers;
            TestChampionship.Prizes = new List<Prizes>()
            {
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 1, Price = 1000 },
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 3, Price = 200 },
                new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 4, Price = 100 }
            };

            Player newAttender = new Player() { Name = "New Test Player" };

            // Didnt participate in championship
            Assert.That(() => logic.GetPlayersPrizeForChampionship(newAttender.Id, TestChampionship.Id), Throws.ArgumentException);
            // Price wasn't set for that place
            Assert.That(() => logic.GetPlayersPrizeForChampionship(AllPlayers[2].Id, TestChampionship.Id), Throws.ArgumentException);
        }

        [Test]
        public void PlayersRankingAttandacesTest()
        {
            // ARRANGE
            DateTime timeOne = new DateTime(2023, 01, 04);
            DateTime timeTwo = new DateTime(2021, 02, 01);

            List<Championship> expectedOne = new List<Championship>
            {
                AllChampionships[1],
                AllChampionships[2]
            };
            List<Championship> expectedTwo = new List<Championship>
            {
                AllChampionships[0]
            };
            // ACT
            var resultOne = logic.PlayersRankingAttandences(TestPlayer.Id, timeOne).ToList();
            var resultTwo = logic.PlayersRankingAttandences(TestPlayer.Id, timeTwo).ToList();

            // ASSERT
            Assert.AreEqual(expectedOne, resultOne);
            Assert.AreEqual(expectedTwo, resultTwo);

            AllChampionships.RemoveAt(0); AllChampionships.RemoveAt(0);
            Assert.AreEqual(logic.PlayersRankingAttandences(TestPlayer.Id), AllChampionships);
        }

        [Test]
        public void GetPlayersPlaceInChampionshipTest()
        {
            Assert.That(logic.GetPlayersPlaceInChampionship(TestPlayer.Id, TestChampionship.Id), Is.EqualTo(4));
        }

        /* out of order
        [Test]
        public void GetPlayersRankTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            Assert.That(logic.GetPlayersRank(TestPlayer.Id, time), Is.EqualTo(2));
        }
        [Test]
        public void GetPlayersInOrderTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            List<Player> result = logic.GetPlayersInOrder(time).ToList();

            List<Player> expected = new List<Player>()
            {
                AllPlayers[2],
                AllPlayers[0],
                AllPlayers[3],
                AllPlayers[1]
            };

            Assert.AreEqual(expected, result);
        }
        */

        [Test]
        public void GetAttendedChampionshipsTest()
        {
            List<Championship> attandedChampships = new List<Championship>();
            attandedChampships.Add(TestChampionship);
            attandedChampships.Add(new Championship("1#PDC World Darts Championship 23#2022.12.15.#2023.01.03.#96#2500000"));
            TestPlayer.AttendedChampionships = attandedChampships;

            var result = logic.GetAttendedChampionships(TestPlayer.Id);
            Assert.AreEqual(result, attandedChampships);
        }

        [Test]
        public void GetAttendedChampionshipsUnsuccessfullyTest()
        {
            Assert.That(() => logic.GetAttendedChampionships(444), Throws.ArgumentNullException);
        }

        [Test]
        public void GetChampionshipAttendersTest()
        {
            List<Player> attenders = new List<Player>()
            {
                AllPlayers[0],
                AllPlayers[3],
                AllPlayers[2]
            };
            TestChampionship.Attenders = attenders;

            var result = logic.GetChampionshipAttenders(TestChampionship.Id);

            Assert.AreEqual(attenders, result);
        }

        [Test]
        public void GetEmptyChampionshipAttendersTest()
        {
            List<Player> attenders = new List<Player>();
            TestChampionship.Attenders = attenders;

            Assert.That(() => logic.GetChampionshipAttenders(TestChampionship.Id), Throws.ArgumentException);
        }

        [Test]
        public void GetChampionshipMissingPlayersTest()
        {
            // ARRANGE
            var AllPlayers = logic.ReadAll().ToList();

            List<Player> attendedPlayers = new List<Player>();
            attendedPlayers.Add(AllPlayers[0]);
            attendedPlayers.Add(AllPlayers[3]);

            Championship ch = new Championship() { Attenders = attendedPlayers };

            // ACT
            var result = logic.GetChampionshipMissingPlayers(ch.Id).ToList();

            // ASSERT
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetTopPlayersFromTheChampionshipTest()
        {
            // ARRANGE
            List<Player> attenders = logic.ReadAll().ToList();
            Championship ch = new Championship() { Attenders = attenders, Id = 1 };

            List<Player> expected = new List<Player>()
            {
                attenders[3],
                attenders[2],
                attenders[1]
            };

            // ACT
            var result = logic.GetTopPlayersFromChampionship(ch.Id, 3).ToList();

            // ASSERT
            Assert.AreEqual(expected, result);
        }
        #endregion
    }
}
