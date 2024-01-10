using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository
{
    public class PlayerRepository : Repository<Player>, IRepository<Player>
    {
        public PlayerRepository(DartsDbContext ctx) : base(ctx)
        {
        }
    }
}
