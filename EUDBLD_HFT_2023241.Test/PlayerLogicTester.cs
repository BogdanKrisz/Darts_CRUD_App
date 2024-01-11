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
        PlayerLogic pl;
        Mock<IRepository<Player>> mockPlayerRepository;
        Mock<IRepository<PlayerChampionship>> mockPlayerChampionshipRepository;

        [SetUp]
        public void Init()
        {
            base.SetAllDataToDefault();

            // MOQ Stuff
            mockPlayerRepository = new Mock<IRepository<Player>>();
            mockPlayerChampionshipRepository = new Mock<IRepository<PlayerChampionship>>();

            // ReadAll
            mockPlayerRepository.Setup(p => p.ReadAll()).Returns(AllPlayers.AsQueryable);
            mockPlayerChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllPlayerChampionships.AsQueryable);

            // Read
            for (int i = 0; i < AllPlayers.Count(); i++)
                mockPlayerRepository.Setup(p => p.Read(i + 1)).Returns(AllPlayers[i]);
            mockPlayerRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            // Delete
            mockPlayerRepository.Setup(p => p.Delete(444)).Throws<ArgumentNullException>();

            pl = new PlayerLogic(mockPlayerRepository.Object, mockPlayerChampionshipRepository.Object);
        }

        #region CRUD TESTS
        // CRUD TESTS
        [Test]
        public void CreatePlayerTest()
        {
            Player newPlayer = new Player() { Name = "New Player" };

            // ACT
            pl.Create(newPlayer);

            // ASSERT
            mockPlayerRepository.Verify(
                p => p.Create(newPlayer),
                Times.Once);
        }

        [Test]
        public void CreatePlayerUnsuccessfullyTest()
        {
            Player newPlayer = new Player() { Name = "Play" };

            Assert.That(() => pl.Create(newPlayer), Throws.ArgumentException);
        }

        [TestCase(1, "PlayerA")]
        [TestCase(2, "PlayerB")]
        [TestCase(3, "PlayerC")]
        [TestCase(4, "PlayerD")]
        public void ReadPlayer(int id, string expected)
        {
            Assert.That(() => pl.Read(id).Name, Is.EqualTo(expected));
        }

        [Test]
        public void ReadPlayerUnsuccessfullyTest()
        {
            Assert.That(() => pl.Read(444).Name, Throws.ArgumentNullException);
        }

        [Test]
        public void UpdatePlayerTest()
        {
            TestPlayer.Name = "New Name";
            pl.Update(TestPlayer);

            mockPlayerRepository.Verify(
                p => p.Update(TestPlayer),
                Times.Once);
        }

        [Test]
        public void UpdatePlayerUnsuccessfullyTest()
        {
            TestPlayer.Name = "New";

            Assert.That(() => pl.Update(TestPlayer), Throws.ArgumentException);
        }

        [Test]
        public void DeletePlayerTest()
        {
            pl.Delete(TestPlayer.Id);

            mockPlayerRepository.Verify(
                p => p.Delete(TestPlayer.Id),
                Times.Once);
        }

        [Test]
        public void DeletePlayerUnsuccessfullyTest()
        {
            Assert.That(() => pl.Delete(444), Throws.ArgumentNullException);
        }
        #endregion

        #region NON CRUD TESTS
        // NON CRUD TEST
        [Test]
        public void GetPlayersInOrderTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            List<PlayerRank> playerRankResults = pl.GetPlayersInOrder(time).ToList();
            List<Player> result = new List<Player>();
            foreach (var item in playerRankResults)
                result.Add(item.P);
            List<Player> expected = new List<Player>()
            {
                AllPlayers[2],
                AllPlayers[0],
                AllPlayers[3],
                AllPlayers[1]
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void PlayersRankingMoneyTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            var result = pl.PlayersRankingMoney(TestPlayer.Id, time);

            // ASSERT
            Assert.That(result, Is.EqualTo(350000));
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

            Assert.That(() => pl.GetPlayersPrizeForChampionship(AllPlayers[0].Id, TestChampionship), Is.EqualTo(100));
            Assert.That(() => pl.GetPlayersPrizeForChampionship(AllPlayers[2].Id, TestChampionship), Is.EqualTo(500));
            Assert.That(() => pl.GetPlayersPrizeForChampionship(AllPlayers[3].Id, TestChampionship), Is.EqualTo(1000));
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
            Assert.That(() => pl.GetPlayersPrizeForChampionship(newAttender.Id, TestChampionship), Throws.ArgumentException);
            // Price wasn't set for that place
            Assert.That(() => pl.GetPlayersPrizeForChampionship(AllPlayers[2].Id, TestChampionship), Throws.ArgumentException);
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
            var resultOne = pl.PlayersRankingAttandences(TestPlayer.Id, timeOne).ToList();
            var resultTwo = pl.PlayersRankingAttandences(TestPlayer.Id, timeTwo).ToList();

            // ASSERT
            Assert.AreEqual(expectedOne, resultOne);
            Assert.AreEqual(expectedTwo, resultTwo);

            AllChampionships.RemoveAt(0); AllChampionships.RemoveAt(0);
            Assert.AreEqual(pl.PlayersRankingAttandences(TestPlayer.Id), AllChampionships);
        }

        [Test]
        public void GetPlayersPlaceInChampionshipTest()
        {
            Assert.That(pl.GetPlayersPlaceInChampionship(TestPlayer.Id, TestChampionship.Id), Is.EqualTo(4));
        }

        [Test]
        public void GetPlayersRankTest()
        {
            // ARRANGE
            DateTime time = new DateTime(2023, 01, 04);

            // ACT
            Assert.That(pl.GetPlayersRank(TestPlayer, time), Is.EqualTo(2));
        }

        [Test]
        public void GetAttendedChampionshipsTest()
        {
            List<Championship> attandedChampships = new List<Championship>();
            attandedChampships.Add(TestChampionship);
            attandedChampships.Add(new Championship("1#PDC World Darts Championship 23#2022.12.15.#2023.01.03.#96#2500000"));
            TestPlayer.AttendedChampionships = attandedChampships;

            var result = pl.GetAttendedChampionships(TestPlayer.Id);
            Assert.AreEqual(result, attandedChampships);
        }

        [Test]
        public void GetAttendedChampionshipsUnsuccessfullyTest()
        {
            Assert.That(() => pl.GetAttendedChampionships(444), Throws.ArgumentNullException);
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

            var result = pl.GetChampionshipAttenders(TestChampionship);

            Assert.AreEqual(attenders, result);
        }

        [Test]
        public void GetEmptyChampionshipAttendersTest()
        {
            List<Player> attenders = new List<Player>();
            TestChampionship.Attenders = attenders;

            Assert.That(() => pl.GetChampionshipAttenders(TestChampionship), Throws.ArgumentException);
        }

        [Test]
        public void GetChampionshipMissingPlayersTest()
        {
            // ARRANGE
            var AllPlayers = pl.ReadAll().ToList();

            List<Player> attendedPlayers = new List<Player>();
            attendedPlayers.Add(AllPlayers[0]);
            attendedPlayers.Add(AllPlayers[3]);

            Championship ch = new Championship() { Attenders = attendedPlayers };

            List<Player> expected = new List<Player>();
            expected.Add(AllPlayers[1]);
            expected.Add(AllPlayers[2]);

            // ACT
            var result = pl.GetChampionshipMissingPlayers(ch).ToList();

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetTopPlayersFromTheChampionshipTest()
        {
            // ARRANGE
            List<Player> attenders = pl.ReadAll().ToList();
            Championship ch = new Championship() { Attenders = attenders, Id = 1 };

            List<Player> expected = new List<Player>()
            {
                attenders[3],
                attenders[2],
                attenders[1]
            };

            // ACT
            var result = pl.GetTopPlayersFromChampionship(ch.Id, 3).ToList();

            // ASSERT
            Assert.AreEqual(expected, result);
        }
        #endregion
    }
}
