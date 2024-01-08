using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IChampionshipLogic
    {
        void Create(Championship item);
        void Delete(int id);
        IQueryable<Player> GetChampionshipParticipants(int championshipId);
        Championship GetHighestPrizePoolChampionship();
        Championship Read(int id);
        IQueryable<Championship> ReadAll();
        void Update(Championship item);
    }
}