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
    public class ChampionshipLogicTester : MyTestClass
    {
        ChampionshipLogic logic;
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

            logic = new ChampionshipLogic(mockChampionshipRepository.Object, mockPlayerChampionshipRepository.Object);
        }

        #region CRUD TESTS
        // CRUD TESTS
        [Test]
        public void CreateChampionshipTest()
        {
            // ARRANGE
            Championship newC = new Championship() { Name = "Test Championship", MaxAttender = 20, PrizePool = 2000, StartDate = new DateTime(2020,10,10), EndDate = new DateTime(2020,10,11) };

            // ACT
            logic.Create(newC);

            // ASSERT
            mockChampionshipRepository.Verify(
                p => p.Create(newC),
                Times.Once);
        }

        [Test]
        public void CreateChampionshipUnsuccessfullyTests()
        {
            // null
            Championship newC = null;
            Assert.That(() => logic.Create(newC), Throws.Exception);

            // name too short
            newC = new Championship() { Name = "Test Championship", MaxAttender = 20, PrizePool = 2000, StartDate = new DateTime(2020, 10, 10), EndDate = new DateTime(2020, 10, 11) };
            newC.Name = "Test";
            Assert.That(() => logic.Create(newC), Throws.ArgumentException);

            // too few attenders
            newC.Name = "Test Championship";
            newC.MaxAttender = 1;
            Assert.That(() => logic.Create(newC), Throws.ArgumentException);

            // prize pool negative or 0
            newC.MaxAttender = 4;
            newC.PrizePool = -500;
            Assert.That(() => logic.Create(newC), Throws.ArgumentException);

            newC.PrizePool = 0;
            Assert.That(() => logic.Create(newC), Throws.ArgumentException);

            newC.PrizePool = 50000;
            newC.EndDate = new DateTime(2020, 09, 10);
            Assert.That(() => logic.Create(newC), Throws.ArgumentException);
        }

        
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ReadPlayerChampionship(int id)
        {
            Assert.That(() => logic.Read(id), Is.EqualTo(TestChampionship));
        }

        [Test]
        public void ReadPlayerChampionshipUnsuccessfullyTest()
        {
            Assert.That(() => logic.Read(444), Throws.ArgumentNullException);
        }

        [Test]
        public void UpdatePlayerChampionshipTest()
        {
            var old = TestChampionship;
            old.Name = "New Championship";
            old.MaxAttender = 20;
            old.StartDate = new DateTime(2023, 10, 10);
            old.EndDate = new DateTime(2024, 01, 01);
            old.PrizePool = 20000;
            logic.Update(old);

            mockChampionshipRepository.Verify(
                p => p.Update(old),
                Times.Once);
        }

        [Test]
        public void UpdatePlayerUnsuccessfullyTest()
        {
            // null
            Championship newC = null;
            Assert.That(() => logic.Update(newC), Throws.Exception);

            // name too short
            var old = TestChampionship;
            old.Name = "Test";
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            // too few attenders
            old.Name = "Test Championship";
            old.MaxAttender = 1;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            // prize pool negative or 0
            old.MaxAttender = 4;
            old.PrizePool = -500;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            old.PrizePool = 0;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            old.PrizePool = 50000;
            old.EndDate = new DateTime(2020, 09, 10);
            Assert.That(() => logic.Update(old), Throws.ArgumentException);
        }
        #endregion

        // non crud
        [Test]
        public void DeletePlayerFromChampionshipTest()
        {
            PlayerChampionship actual = AllPlayerChampionships.First();

            var player = actual.Player;
            var championship = actual.Championship;
            
            logic.DeletePlayerFromChampionship(player.Id, championship.Id);

            mockPlayerChampionshipRepository.Verify(
                p => p.Delete(actual.Id),
                Times.Once);
        }
    }
}
