﻿using EUDBLD_HFT_2023241.Models;
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
        IQueryable<Player> GetChampionshipAttenders(Championship champship);
        IQueryable<Player> GetChampionshipMissingPlayers(Championship champship);
        IEnumerable<PlayerLogic.PlayerRank> GetPlayersInOrder();
        IEnumerable<PlayerLogic.PlayerRank> GetPlayersInOrder(DateTime time);
        int GetPlayersPlaceInChampionship(int playerId, int championshipId);
        int GetPlayersPrizeForChampionship(int playerId, Championship champship);
        int GetPlayersRank(Player player);
        int GetPlayersRank(Player player, DateTime time);
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