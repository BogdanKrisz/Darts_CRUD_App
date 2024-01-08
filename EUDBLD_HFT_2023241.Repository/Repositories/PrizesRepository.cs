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
    internal class PrizesRepository : Repository<Prizes>, IPrizesRepository
    {
        public PrizesRepository(DartsDbContext ctx) : base(ctx)
        {
        }
    }
}
