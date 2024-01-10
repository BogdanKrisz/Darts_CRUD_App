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
    public class PlayerLogicTester
    {
        PlayerLogic pl;
        Mock<IRepository<Player>> mockPlayerRepository;
        Mock<IRepository<Championship>> mockChampionshipRepository;
        Mock<IRepository<Prizes>> mockPrizesRepository;
        Mock<IRepository<PlayerChampionship>> mockPlayerChampionshipRepository;

        [SetUp]
        public void Init()
        {
            var inputPlayerData = new List<Player>()
            {
                new Player("1#PlayerA"),
                new Player("2#PlayerB"),
                new Player("3#PlayerC"),
                new Player("4#PlayerC")
            }.AsQueryable();



            mockPlayerRepository = new Mock<IRepository<Player>>();
            mockChampionshipRepository = new Mock<IRepository<Championship>>();
            mockPrizesRepository = new Mock<IRepository<Prizes>>();
            mockPlayerChampionshipRepository = new Mock<IRepository<PlayerChampionship>>();

            mockPlayerRepository.Setup(p => p.ReadAll()).Returns(inputPlayerData);


            pl = new PlayerLogic(mockPlayerRepository.Object, mockPlayerChampionshipRepository.Object, mockPrizesRepository.Object);
        }

        [Test]
        public void CreateTest()
        {
            Player newPlayer = new Player() { Name = "New Player" };

            pl.Create(newPlayer);

            mockPlayerRepository.Verify(
                m => m.Create(newPlayer),
                Times.Once);
        }

        [Test]
        public void CreateFailedTest()
        {
            Player newPlayer = new Player() { Name = "Play" };
            
            Assert.That(() => pl.Create(newPlayer), Throws.ArgumentException);
        }
    }
}
