﻿using EUDBLD_HFT_2023242.Models;
using EUDBLD_HFT_2023242.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023242.Repository
{
    public class ChampionshipRepository : Repository<Championship>, IRepository<Championship>
    {
        public ChampionshipRepository(DartsDbContext ctx) : base(ctx)
        {
        }
    }
}
