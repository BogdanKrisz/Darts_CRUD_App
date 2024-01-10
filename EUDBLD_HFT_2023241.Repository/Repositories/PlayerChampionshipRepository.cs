using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository
{
    public class PlayerChampionshipRepository : Repository<PlayerChampionship>, IRepository<PlayerChampionship>
    {
        public PlayerChampionshipRepository(DartsDbContext ctx) : base(ctx)
        {
        }
    }
}
