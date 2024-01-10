using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerChampionshipLogic : IPlayerChampionshipLogic
    {
        IRepository<PlayerChampionship> plChRepo;
        IRepository<Championship> cRepo;

        public PlayerChampionshipLogic(IRepository<PlayerChampionship> repo, IRepository<Championship> cRepo)
        {
            this.plChRepo = repo;
            this.cRepo = cRepo;
        }

        public void Create(PlayerChampionship item)
        {
            this.plChRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.plChRepo.Delete(id);
        }

        public PlayerChampionship Read(int id)
        {
            return this.plChRepo.Read(id);
        }

        public IQueryable<PlayerChampionship> ReadAll()
        {
            return this.plChRepo.ReadAll();
        }

        public void Update(PlayerChampionship item)
        {
            this.plChRepo.Update(item);
        }

        // Non crud

        public int GetId(int playerId, int ChampionshipId)
        {
            return this.plChRepo.ReadAll().FirstOrDefault(t => t.ChampionshipId == ChampionshipId && t.PlayerId == playerId).Id;
        }
    }
}
