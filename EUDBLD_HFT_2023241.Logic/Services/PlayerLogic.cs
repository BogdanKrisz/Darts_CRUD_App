using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        IRepository<Player> repo;

        public PlayerLogic(IRepository<Player> repo)
        {
            this.repo = repo;
        }

        public void Create(Player item)
        {
            if (item.Name.Length < 5)
            {
                throw new ArgumentException("The name is to short!");
            }
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Player Read(int id)
        {
            return this.repo.Read(id);
        }

        // ?
        public Player Read(string name)
        {
            return this.repo.ReadAll().FirstOrDefault(t => t.Name == name);
        }
        //

        public IQueryable<Player> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Player item)
        {
            this.repo.Update(item);
        }


        // non crud methods

        // a megadott évszámig számított 2év nyereményei alapján vett ranksor

        public class PlayerRank
        {
            public Player player { get; set; }
            public int wonMoney { get; set; }
            public int rank { get; set; }
        }

        public class RankingsInOrder
        {
            public int Year { get; set; }
            public IQueryable<Player> players { get; set; }
        }

        // 5 db non crud metódus kell

        public IQueryable<Player> GetPlayersInOrder()
        {

        }

        public Player GetPlayerByRank(int rank, DateTime time)
        {

        }

        public Player GetPlayerByRank(int rank)
        {
            this.GetPlayerByRank(rank, DateTime.Now);
        }

        public IQueryable<Championship> GetAttendedChampionships(int id)
        {

        }
        // Melyik tornán nem vett részt adott évben
        // Melyik tornákon vett rész adott évben


    }
}
