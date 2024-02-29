using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        IRepository<Championship> cRepo;

        public PlayerLogic(IRepository<Player> playerRepo, IRepository<PlayerChampionship> plChRepo, IRepository<Championship> cRepo)
        {
            this.playerRepo = playerRepo;
            this.plChRepo = plChRepo;
            this.cRepo = cRepo;
        }

        public void Create(Player item)
        {
            if (item.Name.Length < 5)
                throw new ArgumentException("Name is too short!");

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
            if (item.Name.Length < 5)
                throw new ArgumentException("Name is too short!");

            this.playerRepo.Update(item);
        }


        // non crud methods
        public class PlayerRank
        {
            public Player P { get; set; }
            public int Money { get; set; }
        }

        // Returns the players in ranking order at the given date
        public IEnumerable<Player> GetPlayersInOrder(DateTime time)
        {
            PlayerRank[] resultPlayerRanks = new PlayerRank[playerRepo.ReadAll().Count()];
            int resultIdx = 0;

            foreach (var player in playerRepo.ReadAll())
            {
                resultPlayerRanks[resultIdx++] = new PlayerRank()
                {
                    P = player,
                    Money = PlayersRankingMoney(player.Id, time)
                };
            }
            var orderedPlayerRanks = resultPlayerRanks.OrderByDescending(p => p.Money).AsQueryable();
            var result = new List<Player>();
            foreach (var player in orderedPlayerRanks)
            {
                result.Add(player.P);
            }
            return result;
        }
        public IEnumerable<Player> GetPlayersInOrder()
        {
            return GetPlayersInOrder(DateTime.Now);
        }

        public int PlayersRankingMoney(int playerId, DateTime time)
        {
            int money = 0;
            var rankingChampships = PlayersRankingAttandences(playerId, time);
            foreach (var champship in rankingChampships)
            {
                try { money += GetPlayersPrizeForChampionship(playerId, champship.Id); } catch { }
            }

            return money;
        }
        public int PlayersRankingMoney(int playerId)
        {
            return this.PlayersRankingMoney(playerId, DateTime.Now);
        }

        public int GetPlayersPrizeForChampionship(int playerId, int champshipId)
        {
            int place = plChRepo.ReadAll().FirstOrDefault(pc => pc.PlayerId == playerId && pc.ChampionshipId == champshipId).Place;
            Championship champship = cRepo.Read(champshipId);

            if (!champship.Attenders.Contains(playerRepo.Read(playerId)))
                throw new ArgumentException("This player didn't participate in this championship!");


            var prize = champship.Prizes.FirstOrDefault(p => p.ChampionshipId == champshipId && p.Place == place);

            if (prize == null)
                throw new ArgumentException("This place doesn't have a prize set in the championship!");

            return prize.Price;
        }

        // Get the given player's attended championships in the previous 2 years from the given time
        // Az átadott player -nek az átadott időtől számított előző 2évben résztvett tornáit adja vissza
        public IQueryable<Championship> PlayersRankingAttandences(int playerId, DateTime time)
        {
            return playerRepo.Read(playerId).AttendedChampionships
                            .Where(ac => ac.EndDate > time.AddYears(-2) && ac.EndDate <= time)
                            .AsQueryable();
        }
        public IQueryable<Championship> PlayersRankingAttandences(int playerId)
        {
            return PlayersRankingAttandences(playerId, DateTime.Now);
        }

        // Returns the place of the given player in the given championship
        public int GetPlayersPlaceInChampionship(int playerId, int championshipId)
        {
            return this.plChRepo.ReadAll()
                .FirstOrDefault(t => t.PlayerId == playerId && t.ChampionshipId == championshipId)
                .Place;
        }

        // returns the rank of the given player at the given time
        public int GetPlayersRank(int playerId, DateTime time)
        {
            Player player = playerRepo.Read(playerId);
            List<Player> players = GetPlayersInOrder(time).ToList();
            int i = 0;
            while (i < players.Count() && players[i] != player)
                i++;
            return i + 1;
        }
        public int GetPlayersRank(int playerId)
        {
            return this.GetPlayersRank(playerId, DateTime.Now);
        }

        public IQueryable<Championship> GetAttendedChampionships(int playerId)
        {
            var result = playerRepo.Read(playerId).AttendedChampionships.AsQueryable();
            return result;
        }

        public IQueryable<Player> GetChampionshipAttenders(int champshipId)
        {
            Championship champship = cRepo.Read(champshipId);
            var result = champship.Attenders;
            if (result.Count() < 1)
                throw new ArgumentException("This championship doesnt have any participants!");

            return result.AsQueryable();
        }

        public IQueryable<Player> GetChampionshipMissingPlayers(int champshipId)
        {
            Championship champship = cRepo.Read(champshipId);
            return playerRepo.ReadAll().ToList().Except(champship.Attenders).AsQueryable();
        }

        // Visszaadja az összes játékost aki a paraméterként adott helyezéssel egyenlő vagy jobb eredményt ért el
        public IEnumerable<Player> GetTopPlayersFromChampionship(int championshipId, int worstPlace)
        {
            var result = plChRepo.ReadAll()
                        .Where(t => t.ChampionshipId == championshipId && t.Place <= worstPlace)
                        .OrderBy(t => t.Place)
                        .Select(t => t.Player);

            if (result.Count() < 1)
                throw new ArgumentException("This championship doesnt have any participants!");

            return result;
        }
    }
}
