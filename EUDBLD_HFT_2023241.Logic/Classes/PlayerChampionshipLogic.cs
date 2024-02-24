using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic
{
    public class PlayerChampionshipLogic : IPlayerChampionshipLogic
    {
        IRepository<PlayerChampionship> plChRepo;
        IRepository<Championship> cRepo;

        public PlayerChampionshipLogic(IRepository<PlayerChampionship> repo, IRepository<Championship> cRepo)
        {
            this.plChRepo = repo;
            this.cRepo = cRepo;
        }

        public void Create(PlayerChampionship item)
        {
            var champship = cRepo.Read(item.ChampionshipId);
            if (champship == null)
                throw new ArgumentException("This championship doesn't exist!");

            if (champship.Attenders.Count() == champship.MaxAttender)
                throw new ArgumentException("There is no free space in the championship!");

            if (plChRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.PlayerId == item.PlayerId) != null)
                throw new ArgumentException("This player is already in the championship!");

            if (item.Place > champship.MaxAttender || item.Place < 0)
                throw new ArgumentException("This place is not valid!");

            if (!validNewPlace(item.ChampionshipId, item.Place))
                throw new ArgumentException($"There are too many players already in this place {item.Place}");

            this.plChRepo.Create(item);
        }

        public void Delete(int id)
        {
            this.plChRepo.Delete(id);
        }

        public PlayerChampionship Read(int id)
        {
            return this.plChRepo.Read(id);
        }

        public IQueryable<PlayerChampionship> ReadAll()
        {
            return this.plChRepo.ReadAll();
        }

        public void Update(PlayerChampionship item)
        {
            var champship = cRepo.Read(item.ChampionshipId);

            if (champship == null)
                throw new ArgumentException("This championship doesn't exist!");

            if (champship.Attenders.Count() == champship.MaxAttender)
                throw new ArgumentException("There is no free space in the championship!");

            if (plChRepo.ReadAll().FirstOrDefault(t => t.Championship == champship && t.PlayerId == item.PlayerId && t.Id != item.Id) != null)
                throw new ArgumentException("This player is already in the championship!");

            if (item.Place > champship.MaxAttender || item.Place < 0)
                throw new ArgumentException("This place is not valid!");

            if (!validNewPlace(item.ChampionshipId, item.Place))
                throw new ArgumentException($"There are too many players already in this place {item.Place}");

            this.plChRepo.Update(item);
        }

        // Non crud

        public int GetId(int playerId, int ChampionshipId)
        {
            return this.plChRepo.ReadAll().FirstOrDefault(t => t.ChampionshipId == ChampionshipId && t.PlayerId == playerId).Id;
        }

        // van-e még a bajnokságban a paraméterként megadott szabad helyezés (pl. 1db 1. hely van, de van 4db 5. hely (5-8), )
        bool validNewPlace(int championshipId, int place)
        {
            int numOfPlayersInThatPlace = ReadAll().Where(pc => pc.ChampionshipId == championshipId && pc.Place == place).Count();
            bool valid = false;
            // 1db 1., 1db 2., 2db 3., 4db 5., 8db 9., 16db 17., 32db 33., ....
            if (((place == 1 || place == 2) && numOfPlayersInThatPlace == 0) || (numOfPlayersInThatPlace < place - 1))
            {
                valid = true;
            }
            return valid;
        }
    }
}
