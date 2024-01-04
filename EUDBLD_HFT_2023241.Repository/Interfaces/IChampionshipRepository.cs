﻿using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository.Interfaces
{
    internal interface IChampionshipRepository : IRepository<Championship>
    {
        void AddPlayer(Player player);
        void AddPlayer(ICollection players);
    }
}