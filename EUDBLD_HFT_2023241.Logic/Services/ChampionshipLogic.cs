using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Logic.Services
{
    public class ChampionshipLogic : IChampionshipLogic
    {
        IRepository<Championship> repo;

        public ChampionshipLogic(IRepository<Championship> repo)
        {
            this.repo = repo;
        }

        public void Create(Championship item)
        {
            if (item.MaxAttender < 2)
            {
                throw new ArgumentException("Too few participants!");
            }
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Championship Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<Championship> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Championship item)
        {
            this.repo.Update(item);
        }


        // non crud methods

        // !!!!!!!! felvenni új embert egy championshipbe (playerChampionship logic szerintem ide elég, nemkell neki új logic class)!!! -> csak valid helyezés lehessen (championship nagyságát nézze)
        // !! Checkoljuk, hogy minden metódus szerepel -e az interfacekben
        // ha beírunk valakit egy championshipbe hogy hanyadik, nézzük meg, hogy van-e annyi férőhely (legit-e az elért eredménye)
        // ? nemtom, hogy van -e olyan non crud, hogy top4 t kiírja egy bizonyos championshipre

        // 5 db non crud metódus kell

        public IQueryable<Player> GetChampionshipParticipants()
        {

        }

        public Player GetPlayerByPlace(int place)
        {

        }

        // Returns the number of players given in order starting with the 1st
        public IQueryable<Player> GetTopPlayers(int numberOfPlayers)
        {

        }
    }
}
