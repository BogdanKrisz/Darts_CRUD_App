using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        IRepository<Player> playerRepo;
        IRepository<PlayerChampionship> plChRepo;
        IRepository<Prizes> prizeRepo;
        IRepository<Championship> chRepo;

        public PlayerLogic(IRepository<Player> playerRepo, IRepository<PlayerChampionship> plChRepo, IRepository<Prizes> prizeRepo, IRepository<Championship> chRepo)
        {
            this.playerRepo = playerRepo;
            this.plChRepo = plChRepo;
            this.prizeRepo = prizeRepo;
            this.chRepo = chRepo;
        }

        public void Create(Player item)
        {
            if (item.Name.Length < 5)
            {
                throw new ArgumentException("Name is too short!");
            }
            this.playerRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.playerRepo.Delete(id);
        }

        public Player Read(int id)
        {
            return this.playerRepo.Read(id);
        }

        public IQueryable<Player> ReadAll()
        {
            return this.playerRepo.ReadAll();
        }

        public void Update(Player item)
        {
            this.playerRepo.Update(item);
        }


        // non crud methods

        public class PlayerRank
        {
            public Player P { get; set; }
            public int Money { get; set; }
        }

        // Returns the players in ranking order at the given date
        public IEnumerable<PlayerRank> GetPlayersInOrder(DateTime time)
        {
            PlayerRank[] result = new PlayerRank[playerRepo.ReadAll().Count()];
            int resultIdx = 0;

            foreach (var player in playerRepo.ReadAll())
            {
                result[resultIdx++] = new PlayerRank()
                {
                    P = player,
                    Money = PlayersRankingMoney(player.Id, time)
                };
            }
            return result.OrderByDescending(p => p.Money).AsQueryable();
        }
        public IEnumerable<PlayerRank> GetPlayersInOrder()
        {
            return GetPlayersInOrder(DateTime.Now);
        }

        public int PlayersRankingMoney(int playerId, DateTime time)
        {
            int money = 0;
            var pChampships = PlayersRankingAttandences(playerId, time);
            foreach (var champship in pChampships)
                money += GetPrizeForPlace(champship.Id, GetPlayersPlaceInChampionship(playerId, champship.Id));
            return money;
        }
        public int PlayersRankingMoney(int playerId)
        {
            return this.PlayersRankingMoney(playerId, DateTime.Now);
        }

        // Get the given player's attended championships in the previous 2 years from the given time
        // Az átadott player -nek az átadott időtől számított előző 2évben résztvett tornáit adja vissza
        public IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time)
        {
            CheckPlayerId(playerId);
            return from t in plChRepo.ReadAll()
                   where t.PlayerId == playerId && t.Championship.EndDate > time.AddYears(-2) && t.Championship.EndDate <= time
                   select t.Championship;
        }
        public IQueryable<Championship> PlayersRankingAttandences(int playerId)
        {
            CheckPlayerId(playerId);
            return PlayersRankingAttandences(playerId, DateTime.Now);
        }

        // Returns the place of the given player in the given championship
        public int GetPlayersPlaceInChampionship(int playerId, int championshipId)
        {
            // Ha nem vett részt egy bajnokságban, akkor ne fusson hibára
            CheckPlayerId(playerId);
            return this.plChRepo.ReadAll()
                .FirstOrDefault(t => t.PlayerId == playerId && t.ChampionshipId == championshipId)
                .Place;
        }

        // Returns the prize for the given place in the given championship
        public int GetPrizeForPlace(int championshipId, int place)
        {
            // Ha a lekérdezett helyezés kisebb mint 0
            if (place < 0)
                throw new ArgumentOutOfRangeException("Nincs ilyen helyezés!");
            return this.prizeRepo.ReadAll()
                    .FirstOrDefault(p => p.ChampionshipId == championshipId && p.Place == place)
                    .Price;
        }

        // Gets a rank and a time and gives back a player on that rank on that time
        public Player GetPlayerByRank(int rank, DateTime time)
        {
            IEnumerable<PlayerRank> playersInOrder = GetPlayersInOrder(time);
            if (rank < 0 || rank > playersInOrder.Count())
                throw new ArgumentOutOfRangeException("Nincs ilyen helyezéssel játékos!");
            return playersInOrder.ElementAt(rank).P;
        }
        public Player GetPlayerByRank(int rank)
        {
            return GetPlayerByRank(rank, DateTime.Now);
        }

        // returns the rank of the given player at the given time
        public int GetPlayersRank(int playerId, DateTime time)
        {
            CheckPlayerId(playerId);
            return GetPlayersInOrder(time).ToList().FindIndex(t => t.P.Id == playerId) + 1;
        }
        public int GetPlayersRank(int playerId)
        {
            return this.GetPlayersRank(playerId, DateTime.Now);
        }

        public IQueryable<Championship> GetAttendedChampionships(int playerId)
        {
            CheckPlayerId(playerId);
            return from t in plChRepo.ReadAll()
                   where t.PlayerId == playerId
                   select t.Championship;
        }

        public IQueryable<Championship> GetNOTAttendedChampionships(int playerId)
        {
            CheckPlayerId(playerId);
            return chRepo.ReadAll().Except(GetAttendedChampionships(playerId));
        }

        // Returns the number of players given from a tournament in order starting with the 1st
        // Visszaadja a megadott számú játékost a tornán az elsőtől kezdve lefele
        public IEnumerable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers)
        {
            var result = from t in plChRepo.ReadAll()
                         where t.ChampionshipId == championshipId && t.Place <= numberOfPlayers
                         orderby t.Place
                         select t.Player;

            if (result.Count() < 1)
                throw new ArgumentException("This championship doesnt have any participants!");

            return result.Take(numberOfPlayers) ?? throw new ArgumentNullException("There is no Championship with this ID!");
        }

        // Check if the playid given is valid or not
        bool CheckPlayerId(int playerId)
        {
            if (playerRepo.Read(playerId) == null)
                throw new ArgumentNullException("Nincs ilyen ID-val játékos!");
            return true;
        }
    }
}
