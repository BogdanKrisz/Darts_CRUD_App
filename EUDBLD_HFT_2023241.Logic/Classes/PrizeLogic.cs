using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PrizeLogic : IPrizeLogic
    {
        IRepository<Prizes> pRepo;
        IRepository<Championship> cRepo;

        public PrizeLogic(IRepository<Prizes> pRepo, IRepository<Championship> cRepo)
        {
            this.pRepo = pRepo;
            this.cRepo = cRepo;
        }

        public void Create(Prizes item)
        {
            var champship = cRepo.Read(item.ChampionshipId);

            if (champship == null)
                throw new ArgumentNullException("This championship doesn't exist!");

            if (item.Place <= 0 || item.Place > champship.MaxAttender)
                throw new ArgumentException("This place doesn't exist in the championship!");

            if (item.Price < 0)
                throw new ArgumentException("The prize can't be negative!");

            if (item.Price > champship.PrizePool)
                throw new ArgumentException("The prize can't be more then the whole prize pool!");

            if (pRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.Place == item.Place) != null)
                throw new ArgumentException("This place already has a prize!");

            this.pRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.pRepo.Delete(id);
        }

        public Prizes Read(int id)
        {
            return this.pRepo.Read(id);
        }

        public IQueryable<Prizes> ReadAll()
        {
            return this.pRepo.ReadAll();
        }

        public void Update(Prizes item)
        {
            var champship = cRepo.Read(item.ChampionshipId);

            if (champship == null)
                throw new ArgumentNullException("This championship doesn't exist!");

            if (item.Place <= 0 || item.Place > champship.MaxAttender)
                throw new ArgumentException("This place doesn't exist in the championship!");

            if (item.Price < 0)
                throw new ArgumentException("The prize can't be negative!");

            if (item.Price > champship.PrizePool)
                throw new ArgumentException("The prize can't be more then the whole prize pool!");

            if (pRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.Place == item.Place && t.Id != item.Id) != null)
                throw new ArgumentException("This place already has a prize!");

            this.pRepo.Update(item);
        }

        // non crud

        public IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId)
        {
            return this.pRepo.ReadAll().Where(t => t.ChampionshipId == championshipId);
        }
    }
}
