using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic.Services
{
    public interface IChampionshipLogic
    {
        void Create(Championship item);
        void Delete(int id);
        Championship Read(int id);
        IQueryable<Championship> ReadAll();
        void Update(Championship item);

        IQueryable<Player> GetChampionshipParticipants();
        Player GetPlayerByPlace(int place);
        IQueryable<Player> GetTopPlayers(int numberOfPlayers);
    }
}