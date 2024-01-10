using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using EUDBLD_HFT_2023241.Repository.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Test
{
    [TestFixture]
    public class DartsLogicTester
    {
        [SetUp]
        public void Init()
        {
            
        }

        [Test]
        public void PlayerRankingAttandancesTest()
        {

            var ctx = new DartsDbContext();

            var playerRepo = new PlayerRepository(ctx);
            var championshipRepo = new ChampionshipRepository(ctx);
            var prizesRepo = new PrizesRepository(ctx);
            var playerChampionshipRepo = new PlayerChampionshipRepository(ctx);

            var playerLogic = new PlayerLogic(playerRepo, playerChampionshipRepo, prizesRepo, championshipRepo);
            var championshipLogic = new ChampionshipLogic(championshipRepo, playerChampionshipRepo, playerRepo);
            var prizeLogic = new PrizeLogic(prizesRepo);
            var playerChampionshipLogic = new PlayerChampionshipLogic(playerChampionshipRepo);

            var inOrderResult = playerLogic.PlayersRankingAttandences(1);
            ;
        }

        // Test with different dates
        [Test]
        public void GetAllPlayersInOrderTest()
        {

            var ctx = new DartsDbContext();

            var playerRepo = new PlayerRepository(ctx);
            var championshipRepo = new ChampionshipRepository(ctx);
            var prizesRepo = new PrizesRepository(ctx);
            var playerChampionshipRepo = new PlayerChampionshipRepository(ctx);

            var playerLogic = new PlayerLogic(playerRepo, playerChampionshipRepo, prizesRepo, championshipRepo);
            var championshipLogic = new ChampionshipLogic(championshipRepo, playerChampionshipRepo, playerRepo);
            var prizeLogic = new PrizeLogic(prizesRepo);
            var playerChampionshipLogic = new PlayerChampionshipLogic(playerChampionshipRepo);

            var inOrderResult = playerLogic.GetPlayersInOrder().ToArray();

            int i = 0;
            while (i < inOrderResult.Length - 1 && inOrderResult[i].Money >= inOrderResult[i + 1].Money)
                i++;

            Assert.That(i, Is.EqualTo(inOrderResult.Length - 1));
        }

        [Test]
        public void GetTopXplayers()
        {

            // 6os id tournament = players championship finals 21
            // 1.Hely peter wright
            // 2- 3- 4-
        }       
    }
}
