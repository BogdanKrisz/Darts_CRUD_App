using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class ChampionshipLogic : IChampionshipLogic
    {
        IRepository<Championship> cRepo;
        IRepository<PlayerChampionship> plChRepo;
        IRepository<Player> pRepo;

        public ChampionshipLogic(IRepository<Championship> cRepo, IRepository<PlayerChampionship> plChRepo, IRepository<Player> pRepo)
        {
            this.cRepo = cRepo;
            this.plChRepo = plChRepo;
            this.pRepo = pRepo;
        }

        public void Create(Championship item)
        {
            if (item.MaxAttender < 2)
                throw new ArgumentOutOfRangeException("The tournament has too few max attenders!");
            this.cRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.cRepo.Delete(id);
        }

        public Championship Read(int id)
        {
            return this.cRepo.Read(id);
        }

        public IQueryable<Championship> ReadAll()
        {
            return this.cRepo.ReadAll();
        }

        public void Update(Championship item)
        {
            this.cRepo.Update(item);
        }



        // non crud methods
        public void AddPlayerToChampionship(int championshipId, int playerId, int place)
        {
            if (Read(championshipId).Attenders.Count == Read(championshipId).MaxAttender)
                throw new ArgumentOutOfRangeException("There is no free space in the championship!");

            bool alreadyInside = plChRepo.ReadAll()
                .FirstOrDefault(t => t.ChampionshipId == championshipId && t.PlayerId == playerId) != null;

            if (alreadyInside)
                throw new ArgumentException("This player is already in the championship!");

            if (place > Read(championshipId).MaxAttender || place < 0)
                throw new ArgumentException("This place doesnt exists in the championship!");

            plChRepo.Create(new PlayerChampionship() { ChampionshipId = championshipId, PlayerId = playerId, Place = place });
        }

        public void DeletePlayerFromChampionship(int playerId, int championshipId)
        {
            int playerChampionshipId = this.plChRepo.ReadAll().FirstOrDefault(t => t.ChampionshipId == championshipId && t.PlayerId == playerId).Id;
            plChRepo.Delete(playerChampionshipId);
        }

        public IQueryable<Player> GetChampionshipParticipants(int championshipId, bool inOrder = false)
        {
            var result = from t in plChRepo.ReadAll()
                         where t.ChampionshipId == championshipId
                         select t.Player;
            if (inOrder)
                result = result.OrderBy(t => GetPlayersPlaceInChampionship(t.Id, championshipId));
            return result;
        }

        public IQueryable<Player> GetChampionshipMissingPlayers(int championshipId, bool inOrder = false)
        {
            return pRepo.ReadAll().Except(GetChampionshipParticipants(championshipId));
        }

        // Returns the place of the given player in the given championship
        public int GetPlayersPlaceInChampionship(int playerId, int championshipId)
        {
            return this.plChRepo.ReadAll()
                .FirstOrDefault(t => t.PlayerId == playerId && t.ChampionshipId == championshipId)
                .Place;
        }

        // Returns the championship with the highest prizepool 
        public Championship GetHighestPrizePoolChampionship()
        {
            return this.cRepo.ReadAll()
                .OrderBy(t => t.PrizePool)
                .FirstOrDefault();
        }
    }
}
