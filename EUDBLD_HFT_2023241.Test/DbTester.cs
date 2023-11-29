using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EUDBLD_HFT_2023241.Repository.Data;
using EUDBLD_HFT_2023241.Models;

namespace EUDBLD_HFT_2023241.Test
{
    [TestFixture]
    public class DbTester
    {
        [Test]
        public void DbTest()
        {
            // ARRANGE
            DartsDbContext ctx = new();
            // ACT
            Player[] players = ctx.Players.ToArray();

            Player result = new Player("1#Michael Smith");
            //ASSERT
            Assert.That(result, Is.EqualTo(players[0]));
        }
    }
}
