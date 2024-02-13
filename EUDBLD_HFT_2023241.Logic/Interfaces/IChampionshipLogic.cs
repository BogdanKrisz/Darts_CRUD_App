using EUDBLD_HFT_2023241.Logic.Interfaces;
using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IChampionshipLogic : ILogic<Championship>
    {
        void DeletePlayerFromChampionship(int playerId, int championshipId);
    }
}