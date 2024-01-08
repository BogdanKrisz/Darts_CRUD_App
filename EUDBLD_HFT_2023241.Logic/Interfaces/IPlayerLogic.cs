using EUDBLD_HFT_2023241.Models;
using System;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IPlayerLogic
    {
        void Create(Player item);
        void Delete(int id);
        IQueryable<Championship> GetAttendedChampionships(int playerId);
        Player GetPlayerByRank(int rank);
        Player GetPlayerByRank(int rank, DateTime time);
        IQueryable<PlayerLogic.PlayerRank> GetPlayersInOrder();
        IQueryable<PlayerLogic.PlayerRank> GetPlayersInOrder(DateTime time);
        int GetPlayersPlaceInChampionship(int playerId, int championshipId);
        int GetPrizeForPlace(int championshipId, int place);
        IQueryable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers);
        IQueryable<Championship> PlayersRankingAttandences(int playerId);
        IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time);
        Player Read(int id);
        IQueryable<Player> ReadAll();
        void Update(Player item);
    }
}