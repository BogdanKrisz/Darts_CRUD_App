using EUDBLD_HFT_2023241.Models;
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
        public void test1()
        {
            DartsDbContext ctx = new DartsDbContext();
            
            var players = ctx.Players.ToList();
            var prizes = ctx.Prizes.ToList();
            var champships = ctx.Championships.ToList();
            var playerChampship = ctx.PlayerChampionships.ToList();

            //var places = ctx. ;

            List<Player> attendedPlayers = new List<Player>();
            int specChampshipId = 3;

            var attPlayers = champships.Where(c => c.Id == specChampshipId).SelectMany(ch => ch.Attenders);
            var notAttendedPlayers = players.Except(champships.Where(c => c.Id == specChampshipId).SelectMany(ch => ch.Attenders));

            //var top3ofSpecChampship = champships.Where(c => c.Id == specChampshipId).SelectMany(ch => ch.Attenders).OrderBy(/* */).Take(5);

            int specPlayerId = 3;

            var hanyadikEgyChampShipBen = playerChampship
                .Where(pc => pc.Player.Id == specPlayerId && pc.ChampionshipId == specChampshipId)
                .FirstOrDefault()
                .Place;

            ;
        }

        [Test]
        public void FirstPrizeChampionship()
        {
            DartsDbContext ctx = new DartsDbContext();
            List<Prizes> PDCWorldChampionship = ctx.Prizes.Where(t => t.ChampionshipId == 1).ToList();
            int FirstPrice = PDCWorldChampionship.Find(t => t.Place == 1).Price;
            ;
        }
    }
}
