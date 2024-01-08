﻿using EUDBLD_HFT_2023241.Models;
using System.Linq;

namespace EUDBLD_HFT_2023241.Logic.Services
{
    public interface IPrizeLogic
    {
        void Create(Prizes item);
        void Delete(int id);
        Prizes Read(int id);
        IQueryable<Prizes> ReadAll();
        void Update(Prizes item);

        int GetPrizeForPlace(int championshipId, int place);
        Championship GetHighestPrizePoolChampionship();
    }
}