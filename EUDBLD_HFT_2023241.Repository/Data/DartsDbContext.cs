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
        
        //public DbSet<PlayerChampionship> PlayerChampionships { get; set; }

        public DbSet<Prizes> Prizes { get; set; }

        public DartsDbContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;
                    AttachDbFilename=|DataDirectory|\Darts.mdf;Integrated Security=True";

                builder
                    .UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prizes>(x => x
            .HasOne<Championship>()
            .WithMany()
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
                new Player("11#Dave Chisnall"),
                new Player("12#Dirk van Duijvenbode"),
                new Player("13#James Wade"),
                new Player("14#Joe Cullen"),
                new Player("15#Dimitri Van den Bergh"),
                new Player("16#Ross Smith"),
                new Player("17#Chris Dobey"),
                new Player("18#Stephen Bunting"),
                new Player("19#Ryan Searle"),
                new Player("20#Andrew Gilding"),
                new Player("21#Gary Anderson"),
                new Player("22#Gabriel Clemens"),
                new Player("23#Josh Rock"),
                new Player("24#Krzysztof Ratajski")
            });

            modelBuilder.Entity<Championship>().HasData(new Championship[]
            {
                new Championship("1#PDC World Darts Championship 23#2022.12.15.#2023.01.03.#96#2500000"),
                new Championship("2#Cazoo Players Championship Finals 23#2023.11.24.#2023.11.26.#64#600000"),
                new Championship("3#Grand Slam Of Darts 23#2023.11.12.#2023.11.20.#32#650000")
            });

            modelBuilder.Entity<Prizes>().HasData(new Prizes[]
            {
                new Prizes("1#1#1#500000"),
                new Prizes("2#1#2#200000"),
                new Prizes("3#1#3#100000"),
                new Prizes("4#1#4#100000"),
                new Prizes("5#2#1#120000"),
                new Prizes("6#2#2#60000"),
                new Prizes("7#2#3#30000"),
                new Prizes("8#2#4#30000"),
                new Prizes("9#3#1#150000"),
                new Prizes("10#3#2#70000"),
                new Prizes("11#3#3#50000"),
                new Prizes("12#3#4#50000")
            });
        }
    }
}
