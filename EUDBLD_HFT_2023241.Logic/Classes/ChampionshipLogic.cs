using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class ChampionshipLogic : IChampionshipLogic
    {
        IRepository<Championship> cRepo;
        IRepository<PlayerChampionship> plChRepo;

        public ChampionshipLogic(IRepository<Championship> cRepo, IRepository<PlayerChampionship> plChRepo)
        {
            this.cRepo = cRepo;
            this.plChRepo = plChRepo;
        }

        public void Create(Championship item)
        {
            if (item == null)
                throw new ArgumentNullException("This championship doesn't exist!");

            if (item.Name.Length < 5)
                throw new ArgumentException("The championship's name is too short!");

            if (item.MaxAttender < 2)
                throw new ArgumentException("The tournament has too few max attenders!");

            if (item.PrizePool <= 0)
                throw new ArgumentException("Prize pool can't be less or equal to 0!");

            if (item.StartDate > item.EndDate)
                throw new ArgumentException("Start Date must be earlier than the End Date!");

            this.cRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.cRepo.Delete(id);
        }

        public Championship Read(int id)
        {
            return this.cRepo.Read(id);
        }

        public IQueryable<Championship> ReadAll()
        {
            return this.cRepo.ReadAll();
        }

        public void Update(Championship item)
        {
            if (item == null)
                throw new ArgumentNullException("This championship doesn't exist!");

            if (item.Name.Length < 5)
                throw new ArgumentException("The championship's name is too short!");

            if (item.MaxAttender < 2)
                throw new ArgumentException("The tournament has too few max attenders!");

            if (item.PrizePool <= 0)
                throw new ArgumentException("Prize pool can't be less or equal to 0!");

            if (item.StartDate > item.EndDate)
                throw new ArgumentException("Start Date must be earlier than the End Date!");

            this.cRepo.Update(item);
        }


        // non crud

        public void DeletePlayerFromChampionship(int playerId, int championshipId)
        {
            int playerChampionshipId = this.plChRepo.ReadAll().FirstOrDefault(t => t.ChampionshipId == championshipId && t.PlayerId == playerId).Id;
            plChRepo.Delete(playerChampionshipId);
        }
    }
}
