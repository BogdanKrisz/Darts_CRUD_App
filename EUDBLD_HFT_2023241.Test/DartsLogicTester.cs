using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using EUDBLD_HFT_2023241.Repository.Data;
using Moq;
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
        DartsDbContext ctx;
        PlayerRepository playerRepo;
        ChampionshipRepository championshipRepo;
        PrizesRepository prizesRepo;
        PlayerChampionshipRepository playerChampionshipRepo;

        PlayerLogic playerLogic;
        ChampionshipLogic championshipLogic;
        PrizeLogic prizeLogic;
        PlayerChampionshipLogicTester playerChampionshipLogic;

        [SetUp]
        public void Init()
        {
            
        }

    }
}
