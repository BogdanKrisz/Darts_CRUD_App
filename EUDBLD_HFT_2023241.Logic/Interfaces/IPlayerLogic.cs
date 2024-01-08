using EUDBLD_HFT_2023241.Models;
using System;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic
{
    public interface IPlayerLogic
    {
        void Create(Player item);
        void Delete(int id);
        Player Read(int id);
        Player Read(string name);
        IQueryable<Player> ReadAll();
        void Update(Player item);

        IQueryable<Championship> GetAttendedChampionships(int id);
        Player GetPlayerByRank(int rank);
        Player GetPlayerByRank(int rank, DateTime time);
        IQueryable<Player> GetPlayersInOrder();
    }
}