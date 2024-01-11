using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Test
{
    public abstract class MyTestClass
    {
        protected List<Player> AllPlayers;
        protected List<Championship> AllChampionships;
        protected List<PlayerChampionship> AllPlayerChampionships;
        protected List<Prizes> AllPrizes;

        protected Player TestPlayer;
        protected Championship TestChampionship;

        public MyTestClass()
        {
            SetAllDataToDefault();
        }

        protected void SetAllDataToDefault()
        {
            setPlayers();
            setChampionships();
            setPlayerChampionships();
            AllPrizes = new List<Prizes>();
            setPrizes();
            TestPlayer = AllPlayers[0];
            TestChampionship = AllChampionships[0];
        }

        void setPlayers()
        {
            AllPlayers = new List<Player>()
            {
                new Player("1#PlayerA"),
                new Player("2#PlayerB"),
                new Player("3#PlayerC"),
                new Player("4#PlayerD"),
                new Player("5#PlayerE")
            };
        }

        void setChampionships()
        {
            AllChampionships = new List<Championship>()
            {
                new Championship("1#TestCh 21#2020.12.15.#2021.01.03.#96#900000"),
                new Championship("2#TestCh 22#2021.12.15.#2022.01.03.#96#900000"),
                new Championship("3#TestCh 23#2022.12.15.#2023.01.03.#96#900000"),
                new Championship("4#TestCh 24#2023.12.15.#2024.01.03.#96#900000"),
            };
            for (int i = 0; i < AllPlayers.Count()-1; i++)
            {
                AllPlayers[i].AttendedChampionships = AllChampionships;
            }
                
        }

        void setPlayerChampionships()
        {
            AllPlayerChampionships = new List<PlayerChampionship>()
            {
                // 1. championship
                new PlayerChampionship() { Id = 1, Player = AllPlayers[0], PlayerId = AllPlayers[0].Id, Championship = AllChampionships[0], ChampionshipId = AllChampionships[0].Id, Place = 4 }, // PlayerA -> 1.ID champship -> 4.place
                new PlayerChampionship() { Id = 2, Player = AllPlayers[1], PlayerId = AllPlayers[1].Id, Championship = AllChampionships[0], ChampionshipId = AllChampionships[0].Id, Place = 3 }, // PlayerB -> 1.ID champship -> 3.place
                new PlayerChampionship() { Id = 3, Player = AllPlayers[2], PlayerId = AllPlayers[2].Id, Championship = AllChampionships[0], ChampionshipId = AllChampionships[0].Id, Place = 2 }, // PlayerC -> 1.ID champship -> 2.place
                new PlayerChampionship() { Id = 4, Player = AllPlayers[3], PlayerId = AllPlayers[3].Id, Championship = AllChampionships[0], ChampionshipId = AllChampionships[0].Id, Place = 1 },  // PlayerD -> 1.ID champship -> 1.place
                
                // 2. championship
                new PlayerChampionship() { Id = 5, Player = AllPlayers[0], PlayerId = AllPlayers[0].Id, Championship = AllChampionships[1], ChampionshipId = AllChampionships[1].Id, Place = 3 },
                new PlayerChampionship() { Id = 6, Player = AllPlayers[1], PlayerId = AllPlayers[1].Id, Championship = AllChampionships[1], ChampionshipId = AllChampionships[1].Id, Place = 4 },
                new PlayerChampionship() { Id = 7, Player = AllPlayers[2], PlayerId = AllPlayers[2].Id, Championship = AllChampionships[1], ChampionshipId = AllChampionships[1].Id, Place = 1 },
                new PlayerChampionship() { Id = 8, Player = AllPlayers[3], PlayerId = AllPlayers[3].Id, Championship = AllChampionships[1], ChampionshipId = AllChampionships[1].Id, Place = 2 },  

                // 3. championship
                new PlayerChampionship() { Id = 9, Player = AllPlayers[0], PlayerId = AllPlayers[0].Id, Championship = AllChampionships[2], ChampionshipId = AllChampionships[2].Id, Place = 2 },
                new PlayerChampionship() { Id = 10, Player = AllPlayers[1], PlayerId = AllPlayers[1].Id, Championship = AllChampionships[2], ChampionshipId = AllChampionships[2].Id, Place = 4 },
                new PlayerChampionship() { Id = 11, Player = AllPlayers[2], PlayerId = AllPlayers[2].Id, Championship = AllChampionships[2], ChampionshipId = AllChampionships[2].Id, Place = 1 },
                new PlayerChampionship() { Id = 12, Player = AllPlayers[3], PlayerId = AllPlayers[3].Id, Championship = AllChampionships[2], ChampionshipId = AllChampionships[2].Id, Place = 3 }, 

                // 4. championship
                new PlayerChampionship() { Id = 13, Player = AllPlayers[0], PlayerId = AllPlayers[0].Id, Championship = AllChampionships[3], ChampionshipId = AllChampionships[3].Id, Place = 2 },
                new PlayerChampionship() { Id = 14, Player = AllPlayers[1], PlayerId = AllPlayers[1].Id, Championship = AllChampionships[3], ChampionshipId = AllChampionships[3].Id, Place = 3 },
                new PlayerChampionship() { Id = 15, Player = AllPlayers[2], PlayerId = AllPlayers[2].Id, Championship = AllChampionships[3], ChampionshipId = AllChampionships[3].Id, Place = 4 },
                new PlayerChampionship() { Id = 16, Player = AllPlayers[3], PlayerId = AllPlayers[3].Id, Championship = AllChampionships[3], ChampionshipId = AllChampionships[3].Id, Place = 1 },
            };
        }

        void setPrizes()
        {
            int id = 1;
            for (int i = 0; i < AllChampionships.Count(); i++)
            {
                AllChampionships[i].Attenders = AllPlayers;
                AllChampionships[i].Prizes = new List<Prizes>()
                {
                    new Prizes() { Id = id++, Championship = AllChampionships[i], ChampionshipId = AllChampionships[i].Id, Place = 1, Price = 500000 },
                    new Prizes() { Id = id++, Championship = AllChampionships[i], ChampionshipId = AllChampionships[i].Id, Place = 2, Price = 250000 },
                    new Prizes() { Id = id++, Championship = AllChampionships[i], ChampionshipId = AllChampionships[i].Id, Place = 3, Price = 100000 },
                    new Prizes() { Id = id++, Championship = AllChampionships[i], ChampionshipId = AllChampionships[i].Id, Place = 4, Price = 50000 },
                };
                foreach (var prize in AllChampionships[i].Prizes)
                    AllPrizes.Add(prize);
            }
        }
    }
}
