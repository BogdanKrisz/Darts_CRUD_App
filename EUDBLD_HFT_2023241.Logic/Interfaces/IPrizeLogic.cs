using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IPrizeLogic
    {
        void Create(Prizes item);
        void Delete(int id);
        IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId);
        Prizes Read(int id);
        IQueryable<Prizes> ReadAll();
        void Update(Prizes item);
    }
}