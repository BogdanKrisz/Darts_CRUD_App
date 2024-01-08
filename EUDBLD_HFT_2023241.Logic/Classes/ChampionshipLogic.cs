using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class ChampionshipLogic : IChampionshipLogic
    {
        IRepository<Championship> repo;

        public ChampionshipLogic(IRepository<Championship> repo)
        {
            this.repo = repo;
        }

        public void Create(Championship item)
        {
            if (item.MaxAttender < 2)
                throw new ArgumentOutOfRangeException("Túl kevés résztvevő!");
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Championship Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<Championship> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Championship item)
        {
            this.repo.Update(item);
        }


        // non crud methods

        public IQueryable<Player> GetChampionshipParticipants(int championshipId)
        {
            return this.repo.Read(championshipId).Attenders.AsQueryable() ?? throw new ArgumentNullException("Nincs ilyen ID-val bajnokság!");
        }

        // Returns the championship with the highest prizepool 
        public Championship GetHighestPrizePoolChampionship()
        {
            return this.repo.ReadAll()
                .OrderBy(t => t.PrizePool)
                .FirstOrDefault();
        }
    }
}
