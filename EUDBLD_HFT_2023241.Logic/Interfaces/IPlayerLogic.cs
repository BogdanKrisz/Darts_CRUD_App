using EUDBLD_HFT_2023241.Logic.Interfaces;
using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IPlayerLogic : ILogic<Player>
    {
        IQueryable<Championship> GetAttendedChampionships(int playerId);
        IQueryable<Player> GetChampionshipAttenders(int champshipId);
        IQueryable<Player> GetChampionshipMissingPlayers(int champshipId);
        IEnumerable<Player> GetPlayersInOrder();
        IEnumerable<Player> GetPlayersInOrder(DateTime time);
        int GetPlayersPlaceInChampionship(int playerId, int championshipId);
        int GetPlayersPrizeForChampionship(int playerId, int champshipId);
        int GetPlayersRank(int playerId);
        int GetPlayersRank(int playerId, DateTime time);
        IEnumerable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers);
        IQueryable<Championship> PlayersRankingAttandences(int playerId);
        IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time);
        int PlayersRankingMoney(int playerId);
        int PlayersRankingMoney(int playerId, DateTime time);
    }
}