using EUDBLD_HFT_2023242.Logic.Interfaces;
using EUDBLD_HFT_2023242.Models;
using System.Linq;

namespace EUDBLD_HFT_2023242.Logic
{
    public interface IPrizeLogic : ILogic<Prizes>
    {
        IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId);
    }
}