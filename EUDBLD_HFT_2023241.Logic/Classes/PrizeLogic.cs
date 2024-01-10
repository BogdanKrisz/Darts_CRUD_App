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
            this.pRepo.Update(item);
        }

        // non crud

        public IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId)
        {
            return this.pRepo.ReadAll().Where(t => t.ChampionshipId == championshipId);
        }
    }
}
