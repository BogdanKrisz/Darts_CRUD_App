using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
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
                throw new ArgumentOutOfRangeException("Nem lehet negatív a nyeremény!");
            if (item.Price > item.Championship.PrizePool)
                throw new ArgumentOutOfRangeException("Nagyobb a nyeremény, mint a prize pool!");
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
    }
}
