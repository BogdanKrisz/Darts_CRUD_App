using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        IRepository<Player> playerRepo;
        IRepository<PlayerChampionship> plChRepo;
        IRepository<Prizes> prizeRepo;

        public PlayerLogic(IRepository<Player> playerRepo, IRepository<PlayerChampionship> plChRepo, IRepository<Prizes> prizeRepo)
        {
            this.playerRepo = playerRepo;
            this.plChRepo = plChRepo;
            this.prizeRepo = prizeRepo;
        }

        public void Create(Player item)
        {
            if (item.Name.Length < 5)
            {
                throw new ArgumentException("Túl rövid a név!");
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

        public IQueryable<PlayerRank> GetPlayersInOrder(DateTime time)
        {
            PlayerRank[] result = new PlayerRank[playerRepo.ReadAll().Count()];
            int resultIdx = 0;

            foreach (var player in playerRepo.ReadAll())
            {
                int money = 0;
                var pChampships = PlayersRankingAttandences(player.Id, time);
                foreach (var champship in pChampships)
                {
                    money += GetPrizeForPlace(champship.Id, GetPlayersPlaceInChampionship(player.Id, champship.Id));
                }
                result[resultIdx++] = new PlayerRank()
                {
                    P = player,
                    Money = money
                };
            }

            result.OrderBy(p => p.Money);
            return result.AsQueryable();
        }
        public IQueryable<PlayerRank> GetPlayersInOrder()
        {
            return GetPlayersInOrder(DateTime.Now);
        }

        // Get the given player's attended championships in the previous 2 years from the given time
        // Az átadott player -nek az átadott időtől számított előző 2évben résztvett tornáit adja vissza
        public IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time)
        {
            CheckPlayerId(playerId);
            return this.playerRepo.ReadAll()
                    .SelectMany(t => t.AttendedChampionships)
                    .Where(chs => chs.EndDate >= time.AddYears(-2));
        }
        public IQueryable<Championship> PlayersRankingAttandences(int playerId)
        {
            CheckPlayerId(playerId);
            return PlayersRankingAttandences(playerId, DateTime.Now);
        }

        // Returns the place of the given player in the given championship
        public int GetPlayersPlaceInChampionship(int playerId, int championshipId)
        {
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
                .FirstOrDefault(t => t.ChampionshipId == championshipId && t.Place == place)
                .Price;
        }

        // Gets a rank and a time and gives back a player on that rank on that time
        public Player GetPlayerByRank(int rank, DateTime time)
        {
            IQueryable<PlayerRank> playersInOrder = GetPlayersInOrder(time);
            if (rank < 0 || rank > playersInOrder.Count())
                throw new ArgumentOutOfRangeException("Nincs ilyen helyezéssel játékos!");
            return playersInOrder.ElementAt(rank).P;
        }
        public Player GetPlayerByRank(int rank)
        {
            return GetPlayerByRank(rank, DateTime.Now);
        }

        public IQueryable<Championship> GetAttendedChampionships(int playerId)
        {
            CheckPlayerId(playerId);
            return this.playerRepo.Read(playerId).AttendedChampionships.AsQueryable();
        }

        // Returns the number of players given from a tournament in order starting with the 1st
        // Visszaadja a megadott számú játékost a tornán az elsőtől kezdve lefele
        public IQueryable<Player> GetTopPlayersFromChampionship(int championshipId, int numberOfPlayers)
        {
            var result = from t in plChRepo.ReadAll()
                         where t.ChampionshipId == championshipId
                         orderby t.Place
                         select t.Player;

            if (result.Count() < 1)
                throw new ArgumentException("Ez a torna üres!");

            return result.Take(numberOfPlayers) ?? throw new ArgumentNullException("Nincs ilyen ID-val bajnokság!");
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
