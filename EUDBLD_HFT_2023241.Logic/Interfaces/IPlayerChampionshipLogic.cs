using EUDBLD_HFT_2023242.Logic.Interfaces;
using EUDBLD_HFT_2023242.Models;
using System.Linq;

namespace EUDBLD_HFT_2023242.Logic
{
    public interface IPlayerChampionshipLogic : ILogic<PlayerChampionship>
    {
        int GetId(int playerId, int ChampionshipId);
        PlayerChampionship Read(int id);
    }
}