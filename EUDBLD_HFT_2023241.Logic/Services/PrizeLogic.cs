using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic.Services
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

        // non crud methods

        // Returns the price for the given place in the given championship
        public int GetPrizeForPlace(int championshipId, int place)
        {

        }

        // Returns the championship with the highest prizepool 
        public Championship GetHighestPrizePoolChampionship()
        {

        }
    }
}
