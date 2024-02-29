using EUDBLD_HFT_2023242.Logic;
using EUDBLD_HFT_2023242.Models;
using EUDBLD_HFT_2023242.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023242.Test
{
    public class PrizeLogicTester : MyTestClass
    {
        PrizeLogic logic;
        Mock<IRepository<Prizes>> mockPrizesRepository;
        Mock<IRepository<Championship>> mockChampionshipRepository;

        [SetUp]
        public void Init()
        {
            base.SetAllDataToDefault();

            // MOQ Stuff
            mockPrizesRepository = new Mock<IRepository<Prizes>>();
            mockChampionshipRepository = new Mock<IRepository<Championship>>();

            // ReadAll
            mockChampionshipRepository.Setup(p => p.ReadAll()).Returns(AllChampionships.AsQueryable);
            mockPrizesRepository.Setup(p => p.ReadAll()).Returns(AllPrizes.AsQueryable);

            // Read
            mockPrizesRepository.Setup(p => p.Read(It.IsAny<int>())).Returns(AllPrizes[0]);
            mockPrizesRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            mockChampionshipRepository.Setup(p => p.Read(It.IsAny<int>())).Returns(TestChampionship);
            mockChampionshipRepository.Setup(p => p.Read(444)).Throws<ArgumentNullException>();
            // Delete
            mockPrizesRepository.Setup(p => p.Delete(444)).Throws<ArgumentNullException>();

            logic = new PrizeLogic(mockPrizesRepository.Object, mockChampionshipRepository.Object);
        }
        
        #region CRUD TESTS
        // CRUD TESTS
        [Test]
        public void CreatePrizesTest()
        {
            // ARRANGE
            Prizes newP = new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 6, Price = 100 };

            // ACT
            logic.Create(newP);

            // ASSERT
            mockPrizesRepository.Verify(
                p => p.Create(newP),
                Times.Once);
        }

        [Test]
        public void CreatePrizesUnsuccessfullyTests()
        {
            Prizes newP = null;
            Assert.That(() => logic.Create(newP), Throws.Exception);
            
            newP = new Prizes() { Championship = TestChampionship, ChampionshipId = TestChampionship.Id, Place = 0, Price = 200 };
            Assert.That(() => logic.Create(newP), Throws.ArgumentException);
            
            newP.Place = 10000;
            Assert.That(() => logic.Create(newP), Throws.ArgumentException);

            newP.Place = 6;
            newP.Price = -500;
            Assert.That(() => logic.Create(newP), Throws.ArgumentException);

            newP.Price = 1000000000;
            Assert.That(() => logic.Create(newP), Throws.ArgumentException);

            newP.Price = 1000;
            newP.Place = 1;
            Assert.That(() => logic.Create(newP), Throws.ArgumentException);
        }

        
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ReadPrizesTest(int id)
        {
            Assert.That(() => logic.Read(id), Is.EqualTo(AllPrizes[0]));
        }

        [Test]
        public void ReadPrizesChampionshipUnsuccessfullyTest()
        {
            Assert.That(() => logic.Read(444), Throws.ArgumentNullException);
        }

        [Test]
        public void UpdatePrizesChampionshipTest()
        {
            var old = AllPrizes.First();
            old.Championship = AllChampionships[2];
            old.ChampionshipId = AllChampionships[2].Id;
            old.Place = 5;
            old.Price = 200;
            logic.Update(old);

            mockPrizesRepository.Verify(
                p => p.Update(old),
                Times.Once);
        }

        [Test]
        public void UpdatePrizesUnsuccessfullyTest()
        {
            Prizes old = null;
            Assert.That(() => logic.Update(old), Throws.Exception);

            old = AllPrizes.First();
            old.Place = 0;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            old.Place = 10000;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            old.Place = 6;
            old.Price = -500;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);

            old.Price = 1000000000;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);
            
            old.Price = 1000;
            old.Championship = AllChampionships[0];
            old.ChampionshipId = AllChampionships[0].Id;
            old.Place = 2;
            Assert.That(() => logic.Update(old), Throws.ArgumentException);
        }
        #endregion

        // non crud
        [Test]
        public void GetAllPrizesInChampionshipTest()
        {
            List<Prizes> expected = new List<Prizes>()
            {
                AllPrizes[0],
                AllPrizes[1],
                AllPrizes[2],
                AllPrizes[3]
            };
            var result = logic.GetAllPrizesInChampionship(1);
            Assert.AreEqual(expected, result);
        }
    }
}
