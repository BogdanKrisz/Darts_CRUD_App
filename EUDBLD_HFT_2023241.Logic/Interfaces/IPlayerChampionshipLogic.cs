using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic.Classes
{
    public interface IPlayerChampionshipLogic
    {
        void Create(PlayerChampionship item);
        void Delete(int id);
        PlayerChampionship Read(int id);
        IQueryable<PlayerChampionship> ReadAll();
        void Update(PlayerChampionship item);
    }
}