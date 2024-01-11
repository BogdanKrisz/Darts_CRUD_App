using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using static EUDBLD_HFT_2023241.Logic.PlayerLogic;
using System.Collections.Generic;
using System.Linq;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EUDBLD_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        IPlayerLogic playerLogic;
        IChampionshipLogic cLogic;
        IPlayerChampionshipLogic plChLogic;
        IPrizeLogic prizeLogic;

        public StatController(IPlayerLogic playerLogic, IChampionshipLogic cLogic, IPlayerChampionshipLogic plChLogic, IPrizeLogic prizeLogic)
        {
            this.playerLogic = playerLogic;
            this.cLogic = cLogic;
            this.plChLogic = plChLogic;
            this.prizeLogic = prizeLogic;
        }

        [HttpGet("{date}")]
        public IEnumerable<Player> GetPlayersInOrder(DateTime date)
        {
            return this.playerLogic.GetPlayersInOrder(date);
        }

        [HttpGet]
        public IEnumerable<Player> GetPlayersInOrder()
        {
            return this.playerLogic.GetPlayersInOrder();
        }

        [HttpGet("{time}")]
        public int PlayersRankingMoney(int playerId, DateTime time)
        {
            return this.playerLogic.PlayersRankingMoney(playerId, time);
        }

        [HttpGet]
        public int PlayersRankingMoney(int playerId)
        {
            return this.playerLogic.PlayersRankingMoney(playerId);
        }

        [HttpGet]
        public int GetPlayersPrizeForChampionship(int playerId, int champshipId)
        {
            return this.playerLogic.GetPlayersPrizeForChampionship(playerId, champshipId);
        }

        [HttpGet("{time}")]
        public IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time)
        {
            return this.playerLogic.PlayersRankingAttandences(playerId, time);
        }

        [HttpGet]
        public IQueryable<Championship> PlayersRankingAttandences(int playerId)
        {
            return this.playerLogic.PlayersRankingAttandences(playerId);
        }

        [HttpGet]
        public int GetPlayersPlaceInChampionship(int playerId, int championshipId)
        {
            return this.playerLogic.GetPlayersPlaceInChampionship(playerId, championshipId);
        }

        [HttpGet("{time}")]
        public int GetPlayersRank(int playerId, DateTime time)
        {
            return this.playerLogic.GetPlayersRank(playerId, time);
        }

        [HttpGet]
        public int GetPlayersRank(int playerId)
        {
            return this.playerLogic.GetPlayersRank(playerId);
        }

        [HttpGet]
        public IQueryable<Championship> GetAttendedChampionships(int playerId)
        {
            return this.playerLogic.GetAttendedChampionships(playerId);
        }

        [HttpGet]
        public IQueryable<Player> GetChampionshipAttenders(int championshipId)
        {
            return this.playerLogic.GetChampionshipAttenders(championshipId);
        }

        [HttpGet]
        public IQueryable<Player> GetChampionshipMissingPlayers(int championshipId)
        {
            return this.playerLogic.GetChampionshipMissingPlayers(championshipId);
        }

        [HttpGet]
        public IEnumerable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers)
        {
            return this.playerLogic.GetTopPlayersFromChampionship(championshipId, numberOfPlayers);
        }

        // playerChampionship
        [HttpGet]
        public int GetId(int playerId, int ChampionshipId)
        {
            return this.plChLogic.GetId(playerId, ChampionshipId);
        }
        // championship
        [HttpDelete]
        public void DeletePlayerFromChampionship(int playerId, int championshipId)
        {
            this.cLogic.DeletePlayerFromChampionship(playerId, championshipId);
        }
        // prizes
        [HttpGet]
        public IQueryable<Prizes> GetAllPrizesInChampionship(int championshipId)
        {
            return this.prizeLogic.GetAllPrizesInChampionship(championshipId);
        }

    }
}
