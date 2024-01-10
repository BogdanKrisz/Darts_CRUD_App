using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IChampionshipLogic
    {
        void AddPlayerToChampionship(int championshipId, int playerId, int place);
        void Create(Championship item);
        void Delete(int id);
        void DeletePlayerFromChampionship(int playerId, int championshipId);
        IQueryable<Player> GetChampionshipMissingPlayers(int championshipId, bool inOrder = false);
        IQueryable<Player> GetChampionshipParticipants(int championshipId, bool inOrder = false);
        Championship GetHighestPrizePoolChampionship();
        int GetPlayersPlaceInChampionship(int playerId, int championshipId);
        Championship Read(int id);
        IQueryable<Championship> ReadAll();
        void Update(Championship item);
    }
}