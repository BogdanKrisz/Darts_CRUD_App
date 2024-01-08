using EUDBLD_HFT_2023241.Logic.Interfaces;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        IPlayerRepository playerRepository;
        public PlayerLogic(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }


        // itt valamit kell csinálni a rankingal

        // Get all players in place (money) order
        public IQueryable<Player> GetAllPlayersInRakingOrder()
        {
            return playerRepository.ReadAll()
                .OrderBy(t => t.RankInWorld);
        }

        // Get the 
        public Player GetPlayerByPlace(int place, int year)
        {

            return playerRepository.ReadAll()
                .First(t => t.RankInWorld == place);
        }

        // Get a players place in a specific year
        public IQueryable<Player> GetPlayerByPlace(int place, int year)
        {
            return playerRepository.ReadAll()
                .Where(t => t.RankInWorld <= place)
                .OrderBy(t => t.RankInWorld);
        }

        public Player GetPlayerByName(string name)
        {
            return playerRepository.ReadAll()
                .First(t => t.Name == name);
        }


    }
}
