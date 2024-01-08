using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository.Interfaces
{
    public interface IPlayerRepository : IRepository<Player>
    {
        //IQueryable<Player> GetTopPlayers(int numOfPlayers);
    }
}
