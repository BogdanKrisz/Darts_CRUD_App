using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PrizeLogic : IPrizeLogic
    {
        IRepository<Prizes> repo;

        public PrizeLogic(IRepository<Prizes> repo)
        {
            this.repo = repo;
        }

        public void Create(Prizes item)
        {
            if (item.Price < 0)
                throw new ArgumentOutOfRangeException("Prize cant be negative!");
            if (PlaceAlreadyHasAPrize(item.ChampionshipId, item.Place))
                throw new ArgumentException("This place already has a prize!");
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Prizes Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<Prizes> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Prizes item)
        {
            this.repo.Update(item);
        }

        public IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId)
        {
            return this.repo.ReadAll().Where(t => t.ChampionshipId == championshipId);
        }

        bool PlaceAlreadyHasAPrize(int championshipId, int place)
        {
            return this.repo.ReadAll().FirstOrDefault(t => t.ChampionshipId == championshipId && t.Place == place) != null;
        }
    }
}
