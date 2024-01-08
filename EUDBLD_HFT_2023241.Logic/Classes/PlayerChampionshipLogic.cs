using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic.Classes
{
    public class PlayerChampionshipLogic : IPlayerChampionshipLogic
    {
        IRepository<PlayerChampionship> repo;

        public PlayerChampionshipLogic(IRepository<PlayerChampionship> repo)
        {
            this.repo = repo;
        }

        public void Create(PlayerChampionship item)
        {
            if (item.Championship.Attenders.Count() == item.Championship.MaxAttender)
                throw new ArgumentOutOfRangeException("Nincs több férőhely ebben a bajnokságban!");
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public PlayerChampionship Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<PlayerChampionship> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(PlayerChampionship item)
        {
            this.repo.Update(item);
        }
    }
}
