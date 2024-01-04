using EUDBLD_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Repository.Data
{
    public partial class DartsDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Championship> Championships { get; set; }
        
        public DbSet<PlayerChampionship> PlayerChampionships { get; set; }

        public DbSet<Prizes> Prizes { get; set; }

        public DartsDbContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // lazy loading
            if (!builder.IsConfigured)
            {
                builder
                    .UseInMemoryDatabase("DartsDb")
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Prizes>(x => x
            .HasOne(x => x.Championship)
            .WithMany(x => x.Prizes)
            .HasForeignKey(x => x.ChampionshipId)
            .OnDelete(DeleteBehavior.Cascade)
            );
            
            modelBuilder.Entity<Player>()
                .HasMany(x => x.AttendedChampionships)
                .WithMany(x => x.Attenders)
                .UsingEntity<PlayerChampionship>(
                    x => x.HasOne(x => x.Championship)
                        .WithMany().HasForeignKey(x => x.ChampionshipId).OnDelete(DeleteBehavior.Cascade),
                    x => x.HasOne(x => x.Player)
                        .WithMany().HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Cascade)
                );

            // id # Player name
            modelBuilder.Entity<Player>().HasData(new Player[]
            {
                new Player("1#Michael Smith"),
                new Player("2#Michael van Gerwen"),
                new Player("3#Luke Humphries"),
                new Player("4#Peter Wright"),
                new Player("5#Gerwyn Price"),
                new Player("6#Nathan Aspinall"),
                new Player("7#Danny Noppert"),
                new Player("8#Rob Cross"),
                new Player("9#Jonny Clayton"),
                new Player("10#Damon Heta"),
                new Player("11#James Wade"),
                new Player("12#Joe Cullen"),
                new Player("13#Chris Dobey"),
                new Player("14#Stephen Bunting"),
                new Player("15#Ryan Searle"),
                new Player("16#Gary Anderson"),
                new Player("17#Gabriel Clemens"),
                new Player("18#Josh Rock")
            });
            
            // id # Championship name # start date # end date # prizepool
            modelBuilder.Entity<Championship>().HasData(new Championship[]
            {
                new Championship("1#PDC World Darts Championship 23#2022.12.15.#2023.01.03.#96#2500000"),
                new Championship("2#Cazoo Players Championship Finals 23#2023.11.24.#2023.11.26.#64#600000"),
                new Championship("3#Grand Slam Of Darts 23#2023.11.12.#2023.11.20.#32#650000")
            });

            // id # playerId # championshipId # place
            modelBuilder.Entity<PlayerChampionship>().HasData(new PlayerChampionship[]
            {
                new PlayerChampionship("1#1#1#1"),
                new PlayerChampionship("2#2#1#2"),
                new PlayerChampionship("3#3#1#9"),
                new PlayerChampionship("4#4#1#17"),
                new PlayerChampionship("5#5#1#5"),
                new PlayerChampionship("6#6#1#17"),
                new PlayerChampionship("7#7#1#17"),
                new PlayerChampionship("8#8#1#9"),
                new PlayerChampionship("9#9#1#5"),
                new PlayerChampionship("10#10#1#17"),
                new PlayerChampionship("11#11#1#33"),
                new PlayerChampionship("12#12#1#9"),
                new PlayerChampionship("13#13#1#5"),
                new PlayerChampionship("14#14#1#5"),
                new PlayerChampionship("15#15#1#17"),
                new PlayerChampionship("16#16#1#17"),
                new PlayerChampionship("17#17#1#3"),
                new PlayerChampionship("18#18#1#9"),

                new PlayerChampionship("19#1#2#33"),
                new PlayerChampionship("20#2#2#2"),
                new PlayerChampionship("21#3#2#1"),
                new PlayerChampionship("22#5#2#17"),
                new PlayerChampionship("23#8#2#17"),
                new PlayerChampionship("24#9#2#17"),
                new PlayerChampionship("25#10#2#5"),
                new PlayerChampionship("26#11#2#5"),
                new PlayerChampionship("27#12#2#17"),
                new PlayerChampionship("28#13#2#17"),
                new PlayerChampionship("29#14#2#5"),
                new PlayerChampionship("30#15#2#9"),
                new PlayerChampionship("31#16#2#17"),
                new PlayerChampionship("32#17#2#3"),
                new PlayerChampionship("33#18#2#17"),

                new PlayerChampionship("34#1#3#17"),
                new PlayerChampionship("35#2#3#9"),
                new PlayerChampionship("36#3#3#1"),
                new PlayerChampionship("37#4#3#25"),
                new PlayerChampionship("38#5#3#9"),
                new PlayerChampionship("39#6#3#9"),
                new PlayerChampionship("40#7#3#9"),
                new PlayerChampionship("41#8#3#2"),
                new PlayerChampionship("42#9#3#17"),
                new PlayerChampionship("43#10#3#5"),
                new PlayerChampionship("44#11#3#3"),
                new PlayerChampionship("45#13#3#9"),
                new PlayerChampionship("46#14#3#3"),
                new PlayerChampionship("47#15#3#9"),
                new PlayerChampionship("48#16#3#5"),
                new PlayerChampionship("49#18#3#5")
            });

            // id # ChampionshipId # Place # Price
            modelBuilder.Entity<Prizes>().HasData(new Prizes[]
            {
                // 1. Championship
                new Prizes("1#1#1#500000"),
                new Prizes("2#1#2#200000"),
                new Prizes("3#1#3#100000"),
                new Prizes("4#1#5#50000"),
                new Prizes("5#1#9#35000"),
                new Prizes("6#1#17#25000"),
                new Prizes("7#1#33#15000"),
                new Prizes("8#1#65#7500"),

                // 2. Championship
                new Prizes("9#2#1#120000"),
                new Prizes("10#2#2#60000"),
                new Prizes("11#2#3#30000"),
                new Prizes("12#2#5#20000"),
                new Prizes("13#2#9#10000"),
                new Prizes("14#2#17#6500"),
                new Prizes("15#2#33#3000"),

                // 3. Championship
                new Prizes("16#3#1#150000"),
                new Prizes("17#3#2#70000"),
                new Prizes("18#3#3#50000"),
                new Prizes("19#3#5#25000"),
                new Prizes("20#3#9#12250"),
                new Prizes("21#3#17#8000"),
                new Prizes("22#3#25#5000")
            });
            
            // Lekérdezések: Ha nincs olyan place a priceban, akkor nem számoljuk
            // Bajnokságból a top4 lekérdezhető legyen
        }
    }
}
