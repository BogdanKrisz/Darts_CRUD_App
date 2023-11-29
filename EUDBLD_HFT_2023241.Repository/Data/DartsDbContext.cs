using EUDBLD_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository.Data
{
    internal class DartsDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Championship> Championships { get; set; }

        public DartsDbContext()
        {
            this.Database.EnsureCreated();
        }


        }
    }
}
