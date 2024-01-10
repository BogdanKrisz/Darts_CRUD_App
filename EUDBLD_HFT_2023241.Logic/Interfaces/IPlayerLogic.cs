using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IPlayerLogic
    {
        void Create(Player item);
        void Delete(int id);
        IQueryable<Championship> GetAttendedChampionships(int playerId);
        IQueryable<Championship> GetNOTAttendedChampionships(int playerId);
        Player GetPlayerByRank(int rank);
        Player GetPlayerByRank(int rank, DateTime time);
        IEnumerable<PlayerLogic.PlayerRank> GetPlayersInOrder();
        IEnumerable<PlayerLogic.PlayerRank> GetPlayersInOrder(DateTime time);
        int GetPlayersPlaceInChampionship(int playerId, int championshipId);
        int GetPlayersRank(int playerId);
        int GetPlayersRank(int playerId, DateTime time);
        int GetPrizeForPlace(int championshipId, int place);
        IEnumerable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers);
        IQueryable<Championship> PlayersRankingAttandences(int playerId);
        IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time);
        int PlayersRankingMoney(int playerId);
        int PlayersRankingMoney(int playerId, DateTime time);
        Player Read(int id);
        IQueryable<Player> ReadAll();
        void Update(Player item);
    }
}