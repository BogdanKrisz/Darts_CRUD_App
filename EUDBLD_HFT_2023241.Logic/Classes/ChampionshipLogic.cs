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
