using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Test
{
    public class PlayerChampionshipLogicTester : MyTestClass
    {
        PlayerChampionshipLogic logic;
        Mock<IRepository<Championship>> mockChampionshipRepository;
        Mock<IRepository<PlayerChampionship>> mockPlayerChampionshipRepository;

        [SetUp]
        public void Init()
        {
            base.SetAllDataToDefault();

            // MOQ Stuff
            mockChampionshipRepository = new Mock<IRepository<Championship>>();
            mockPlayerChampionshipRepository = new Mock<IRepository<PlayerChampionship>>();

            // ReadAll
            mockChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllChampionships.AsQueryable);
            mockPlayerChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllPlayerChampionships.AsQueryable);

            // Read
            mockPlayerChampionshipRepository.Setup(p => p.Read(It.IsAny<int>())).Returns(AllPlayerChampionships[0]);
            mockPlayerChampionshipRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            mockChampionshipRepository.Setup(p => p.Read(It.IsAny<int>())).Returns(TestChampionship);
            mockChampionshipRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            // Delete
            mockPlayerChampionshipRepository.Setup(p => p.Delete(444)).Throws<ArgumentNullException>();

            logic = new PlayerChampionshipLogic(mockPlayerChampionshipRepository.Object);
        }

        #region CRUD TESTS
        // CRUD TESTS
        [Test]
        public void CreatePlayerChampionshipTest()
        {
            Player newPlayer = new Player() { Name = "New Test Player"};
            PlayerChampionship newPlayerChampionship = new PlayerChampionship() { ChampionshipId = TestChampionship.Id, Championship = TestChampionship, Place = 5, Player = newPlayer, PlayerId = newPlayer.Id };

            // ACT
            logic.Create(newPlayerChampionship);

            // ASSERT
            mockPlayerChampionshipRepository.Verify(
                p => p.Create(newPlayerChampionship),
                Times.Once);
        }
        
        [Test]
        public void CreatePlayerChampionshipUnsuccessfullyTests()
        {
            Player newPlayer = new Player() { Name = "New Test Player" };
            // Champship doesnt exist
            PlayerChampionship newPlayerChampionship = new PlayerChampionship() { ChampionshipId = TestChampionship.Id, Championship = null, Place = 5, Player = newPlayer, PlayerId = newPlayer.Id };
            Assert.That(() => logic.Create(newPlayerChampionship), Throws.ArgumentException);

            // no free space in champship
            newPlayerChampionship.Championship = TestChampionship;
            TestChampionship.MaxAttender = TestChampionship.Attenders.Count();
            Assert.That(() => logic.Create(newPlayerChampionship), Throws.ArgumentException);

            // player is already in the champship
            //if (plChRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.PlayerId == item.PlayerId) != null)
            TestChampionship.MaxAttender = 100;
            newPlayerChampionship = AllPlayerChampionships.First();
            Assert.That(() => logic.Create(newPlayerChampionship), Throws.ArgumentException);

            // place is negative
            newPlayerChampionship = new PlayerChampionship() { ChampionshipId = TestChampionship.Id, Championship = TestChampionship, Place = 5, Player = newPlayer, PlayerId = newPlayer.Id };
            newPlayerChampionship.Place = -1;
            Assert.That(() => logic.Create(newPlayerChampionship), Throws.ArgumentException);

            // place is bigger than maxattanders
            newPlayerChampionship.Place = 101;
            Assert.That(() => logic.Create(newPlayerChampionship), Throws.ArgumentException);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ReadPlayerChampionship(int id)
        {
            Assert.That(() => logic.Read(id), Is.EqualTo(AllPlayerChampionships[0]));
        }

        [Test]
        public void ReadPlayerChampionshipUnsuccessfullyTest()
        {
            Assert.That(() => logic.Read(444), Throws.ArgumentNullException);
        }
        
        [Test]
        public void UpdatePlayerChampionshipTest()
        {
            var old = AllPlayerChampionships.First();
            old.Place = 8;
            logic.Update(old);

            mockPlayerChampionshipRepository.Verify(
                p => p.Update(old),
                Times.Once);
        }

        [Test]
        public void UpdatePlayerUnsuccessfullyTest()
        {
            var old = AllPlayerChampionships.First();
            Player newPlayer = new Player() { Name = "New Test Player" };

            // Champship doesnt exist
            old = new PlayerChampionship() { ChampionshipId = TestChampionship.Id, Championship = null, Place = 5, Player = newPlayer, PlayerId = newPlayer.Id };
            Assert.That(() => logic.Update(old), Throws.ArgumentException);
            
            // no free space in champship
            old.Championship = TestChampionship;
            TestChampionship.MaxAttender = TestChampionship.Attenders.Count();
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            // player is already in the champship
            //if (plChRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.PlayerId == item.PlayerId) != null)
            TestChampionship.MaxAttender = 100;
            old = AllPlayerChampionships.First();
            old.Championship = AllPlayerChampionships[2].Championship;
            old.PlayerId = AllPlayerChampionships[2].PlayerId;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            // place is negative
            old = new PlayerChampionship() { ChampionshipId = TestChampionship.Id, Championship = TestChampionship, Place = 5, Player = newPlayer, PlayerId = newPlayer.Id };
            old.Place = -1;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            // place is bigger than maxattanders
            old.Place = 101;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);
        }
        
        [Test]
        public void DeletePlayerTest()
        {
            logic.Delete(TestPlayer.Id);

            mockPlayerChampionshipRepository.Verify(
                p => p.Delete(TestPlayer.Id),
                Times.Once);
        }

        [Test]
        public void DeletePlayerUnsuccessfullyTest()
        {
            Assert.That(() => logic.Delete(444), Throws.ArgumentNullException);
        }
        #endregion

        // non crud
        [Test]
        public void GetIdTest()
        {
            var plCh = AllPlayerChampionships.First();
            var expected = plCh.Id;
            Assert.That(() => logic.GetId(1, 1), Is.EqualTo(expected));
        }

        [Test]
        public void GetIdUnsuccessfullyTest()
        {
            Assert.That(() => logic.GetId(44,44), Throws.Exception);
        }
    }
}
