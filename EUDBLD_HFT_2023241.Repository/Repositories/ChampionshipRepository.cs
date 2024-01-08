using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository.Data;
using EUDBLD_HFT_2023241.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository.Repositories
{
    internal class ChampionshipRepository : Repository<Championship>, IRepository<Championship>
    {
        public ChampionshipRepository(DartsDbContext ctx) : base(ctx)
        {
        }
    }
}
