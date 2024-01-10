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
                new Player("7#Rob Cross"),
                new Player("8#Jonny Clayton"),
                new Player("9#James Wade"),
                new Player("10#Joe Cullen"),
                new Player("11#Chris Dobey"),
                new Player("12#Gary Anderson"),
                new Player("13#Gabriel Clemens"),
                new Player("14#Luke Littler")
            });
            
            // id # Championship name # start date # end date # maxAttenders # prizepool
            modelBuilder.Entity<Championship>().HasData(new Championship[]
            {
                // 2021
                new Championship("1#World Darts Championship 2021#2020.12.15.#2021.01.03.#96#2500000"),
                new Championship("2#UK Open 2021#2021.03.05.#2021.03.07.#160#450000"),
                new Championship("3#World Matchplay 2021#2021.07.17.#2021.07.25.#32#700000"),
                new Championship("4#World Grand Prix 2021#2021.10.03.#2021.10.09.#32#450000"),
                new Championship("5#European Championship 2021#2021.10.14.#2021.10.17.#32#500000"),
                new Championship("6#Grand Slam of Darts 2021#2021.11.13.#2021.11.21.#32#550000"),
                new Championship("7#Players Championship Finals 2021#2021.11.26.#2021.11.28.#64#500000"),

                // 2022
                new Championship("8#World Darts Championship 2022#2021.12.15.#2022.01.03.#96#2500000"),
                new Championship("9#UK Open 2022#2022.03.04.#2022.03.06.#160#450000"),
                new Championship("10#World Matchplay 2022#2022.07.16.#2022.07.24.#32#800000"),
                new Championship("11#World Grand Prix 2022#2022.10.03.#2022.10.09.#32#600000"),
                new Championship("12#European Championship 2022#2022.10.27.#2022.10.30.#32#500000"),
                new Championship("13#Grand Slam of Darts 2022#2022.11.12.#2022.11.20.#32#650000"),
                new Championship("14#Players Championship Finals 2022#2022.11.25.#2022.11.27.#64#500000"),

                // 2023
                new Championship("15#World Darts Championship 2023#2022.12.15.#2023.01.03.#96#2500000"),
                new Championship("16#UK Open 2023#2023.03.03.#2023.03.05.#160#600000"),
                new Championship("17#World Matchplay 2023#2023.07.15.#2023.07.23.#32#800000"),
                new Championship("18#World Grand Prix 2023#2023.10.02.#2023.10.08.#32#600000"),
                new Championship("19#European Championship 2023#2023.10.26.#2023.10.29.#32#500000"),
                new Championship("20#Grand Slam of Darts 2023#2023.11.11.#2023.11.19.#32#650000"),
                new Championship("21#Players Championship Finals 2023#2023.11.24.#2023.11.26.#64#600000"),

                // 2024
                new Championship("22#World Darts Championship 2024#2023.12.15.#2024.01.03.#96#2500000")
            });

            // id # playerId # championshipId # place
            modelBuilder.Entity<PlayerChampionship>().HasData(new PlayerChampionship[]
            {
                //Michael Smith
                new PlayerChampionship("1#1#1#33"),
                new PlayerChampionship("2#1#2#17"),
                new PlayerChampionship("3#1#3#5"),
                new PlayerChampionship("4#1#4#17"),
                new PlayerChampionship("5#1#5#17"),
                new PlayerChampionship("6#1#6#3"),
                new PlayerChampionship("7#1#7#17"),
                new PlayerChampionship("8#1#8#2"),
                new PlayerChampionship("9#1#9#2"),
                new PlayerChampionship("10#1#10#9"),
                new PlayerChampionship("11#1#11#17"),
                new PlayerChampionship("12#1#12#2"),
                new PlayerChampionship("13#1#13#1"),
                new PlayerChampionship("14#1#14#33"),
                new PlayerChampionship("15#1#15#1"),
                new PlayerChampionship("16#1#16#17"),
                new PlayerChampionship("17#1#17#9"),
                new PlayerChampionship("18#1#18#3"),
                new PlayerChampionship("19#1#19#9"),
                new PlayerChampionship("20#1#20#17"),
                new PlayerChampionship("21#1#21#33"),
                new PlayerChampionship("22#1#22#9"),
                //Michael van Gerwen
                new PlayerChampionship("23#2#1#5"),
                new PlayerChampionship("24#2#2#3"),
                new PlayerChampionship("25#2#3#3"),
                new PlayerChampionship("26#2#4#17"),
                new PlayerChampionship("27#2#5#2"),
                new PlayerChampionship("28#2#6#5"),
                new PlayerChampionship("29#2#7#5"),
                new PlayerChampionship("30#2#8#17"),
                new PlayerChampionship("31#2#9#17"),
                new PlayerChampionship("32#2#10#1"),
                new PlayerChampionship("33#2#11#1"),
                new PlayerChampionship("34#2#12#17"),
                new PlayerChampionship("35#2#13#5"),
                new PlayerChampionship("36#2#14#1"),
                new PlayerChampionship("37#2#15#2"),
                new PlayerChampionship("38#2#16#2"),
                new PlayerChampionship("39#2#17#17"),
                new PlayerChampionship("40#2#18#9"),
                new PlayerChampionship("41#2#19#5"),
                new PlayerChampionship("42#2#20#9"),
                new PlayerChampionship("43#2#21#2"),
                new PlayerChampionship("44#2#22#5"),
                //Luke Humphries
                new PlayerChampionship("45#3#1#65"),
                new PlayerChampionship("46#3#2#2"),
                new PlayerChampionship("47#3#3#9"),
                new PlayerChampionship("48#3#4#9"),
                new PlayerChampionship("49#3#5#9"),
                new PlayerChampionship("50#3#6#17"),
                new PlayerChampionship("51#3#7#9"),
                new PlayerChampionship("52#3#8#5"),
                new PlayerChampionship("53#3#9#33"),
                new PlayerChampionship("54#3#10#17"),
                new PlayerChampionship("55#3#11#17"),
                new PlayerChampionship("56#3#12#5"),
                new PlayerChampionship("57#3#13#3"),
                new PlayerChampionship("58#3#14#3"),
                new PlayerChampionship("59#3#15#9"),
                new PlayerChampionship("60#3#16#9"),
                new PlayerChampionship("61#3#17#3"),
                new PlayerChampionship("62#3#18#1"),
                new PlayerChampionship("63#3#19#5"),
                new PlayerChampionship("64#3#20#1"),
                new PlayerChampionship("65#3#21#1"),
                new PlayerChampionship("66#3#22#1"),
                //Peter Wright
                new PlayerChampionship("67#4#1#17"),
                new PlayerChampionship("68#4#2#33"),
                new PlayerChampionship("69#4#3#1"),
                new PlayerChampionship("70#4#4#17"),
                new PlayerChampionship("71#4#5#17"),
                new PlayerChampionship("72#4#6#2"),
                new PlayerChampionship("73#4#7#1"),
                new PlayerChampionship("74#4#8#1"),
                new PlayerChampionship("75#4#9#9"),
                new PlayerChampionship("76#4#10#5"),
                new PlayerChampionship("77#4#11#5"),
                new PlayerChampionship("78#4#12#5"),
                new PlayerChampionship("79#4#13#17"),
                new PlayerChampionship("80#4#15#17"),
                new PlayerChampionship("81#4#16#9"),
                new PlayerChampionship("82#4#17#9"),
                new PlayerChampionship("83#4#18#5"),
                new PlayerChampionship("84#4#19#1"),
                new PlayerChampionship("85#4#20#25"),
                new PlayerChampionship("86#4#22#33"),
                //Gerwyn Price
                new PlayerChampionship("87#5#1#1"),
                new PlayerChampionship("88#5#2#5"),
                new PlayerChampionship("89#5#3#5"),
                new PlayerChampionship("90#5#4#2"),
                new PlayerChampionship("91#5#5#5"),
                new PlayerChampionship("92#5#6#1"),
                new PlayerChampionship("93#5#7#9"),
                new PlayerChampionship("94#5#8#5"),
                new PlayerChampionship("95#5#9#5"),
                new PlayerChampionship("96#5#10#2"),
                new PlayerChampionship("97#5#11#5"),
                new PlayerChampionship("98#5#12#17"),
                new PlayerChampionship("99#5#13#5"),
                new PlayerChampionship("100#5#14#33"),
                new PlayerChampionship("101#5#15#5"),
                new PlayerChampionship("102#5#16#17"),
                new PlayerChampionship("103#5#17#9"),
                new PlayerChampionship("104#5#18#2"),
                new PlayerChampionship("105#5#19#5"),
                new PlayerChampionship("106#5#20#9"),
                new PlayerChampionship("107#5#21#17"),
                new PlayerChampionship("108#5#22#17"),
                //Nathan Aspinall
                new PlayerChampionship("109#6#1#17"),
                new PlayerChampionship("110#6#2#33"),
                new PlayerChampionship("111#6#3#5"),
                new PlayerChampionship("112#6#4#17"),
                new PlayerChampionship("113#6#5#5"),
                new PlayerChampionship("114#6#7#17"),
                new PlayerChampionship("115#6#8#17"),
                new PlayerChampionship("116#6#9#33"),
                new PlayerChampionship("117#6#10#5"),
                new PlayerChampionship("118#6#11#2"),
                new PlayerChampionship("119#6#12#17"),
                new PlayerChampionship("120#6#13#2"),
                new PlayerChampionship("121#6#14#33"),
                new PlayerChampionship("122#6#15#17"),
                new PlayerChampionship("123#6#16#5"),
                new PlayerChampionship("124#6#17#1"),
                new PlayerChampionship("125#6#18#17"),
                new PlayerChampionship("126#6#19#9"),
                new PlayerChampionship("127#6#20#9"),
                new PlayerChampionship("128#6#22#33"),
                //Rob Cross
                new PlayerChampionship("129#7#1#33"),
                new PlayerChampionship("130#7#2#17"),
                new PlayerChampionship("131#7#3#9"),
                new PlayerChampionship("132#7#4#9"),
                new PlayerChampionship("133#7#5#1"),
                new PlayerChampionship("134#7#6#5"),
                new PlayerChampionship("135#7#7#9"),
                new PlayerChampionship("136#7#8#9"),
                new PlayerChampionship("137#7#9#33"),
                new PlayerChampionship("138#7#10#9"),
                new PlayerChampionship("139#7#11#17"),
                new PlayerChampionship("140#7#12#17"),
                new PlayerChampionship("141#7#13#9"),
                new PlayerChampionship("142#7#14#2"),
                new PlayerChampionship("143#7#15#9"),
                new PlayerChampionship("144#7#16#5"),
                new PlayerChampionship("145#7#17#17"),
                new PlayerChampionship("146#7#18#17"),
                new PlayerChampionship("147#7#19#9"),
                new PlayerChampionship("148#7#20#2"),
                new PlayerChampionship("149#7#21#17"),
                new PlayerChampionship("150#7#22#5"),
                //Jonny Clayton
                new PlayerChampionship("151#8#1#17"),
                new PlayerChampionship("152#8#2#9"),
                new PlayerChampionship("153#8#3#9"),
                new PlayerChampionship("154#8#4#1"),
                new PlayerChampionship("155#8#6#5"),
                new PlayerChampionship("156#8#7#5"),
                new PlayerChampionship("157#8#8#9"),
                new PlayerChampionship("158#8#9#9"),
                new PlayerChampionship("159#8#10#17"),
                new PlayerChampionship("160#8#11#9"),
                new PlayerChampionship("161#8#12#9"),
                new PlayerChampionship("162#8#13#9"),
                new PlayerChampionship("163#8#14#5"),
                new PlayerChampionship("164#8#15#5"),
                new PlayerChampionship("165#8#16#9"),
                new PlayerChampionship("166#8#17#2"),
                new PlayerChampionship("167#8#18#17"),
                new PlayerChampionship("168#8#19#17"),
                new PlayerChampionship("169#8#20#17"),
                new PlayerChampionship("170#8#21#17"),
                new PlayerChampionship("171#8#22#9"),
                //James Wade
                new PlayerChampionship("172#9#1#17"),
                new PlayerChampionship("173#9#2#1"),
                new PlayerChampionship("174#9#3#17"),
                new PlayerChampionship("175#9#4#9"),
                new PlayerChampionship("176#9#5#9"),
                new PlayerChampionship("177#9#6#5"),
                new PlayerChampionship("178#9#7#9"),
                new PlayerChampionship("179#9#8#5"),
                new PlayerChampionship("180#9#9#5"),
                new PlayerChampionship("181#9#10#9"),
                new PlayerChampionship("182#9#11#17"),
                new PlayerChampionship("183#9#12#9"),
                new PlayerChampionship("184#9#14#17"),
                new PlayerChampionship("185#9#15#33"),
                new PlayerChampionship("186#9#16#33"),
                new PlayerChampionship("187#9#17#17"),
                new PlayerChampionship("188#9#18#17"),
                new PlayerChampionship("189#9#19#2"),
                new PlayerChampionship("190#9#20#5"),
                new PlayerChampionship("191#9#21#5"),
                new PlayerChampionship("192#9#22#33"),
                //Joe Cullen
                new PlayerChampionship("193#10#1#9"),
                new PlayerChampionship("194#10#2#33"),
                new PlayerChampionship("195#10#3#9"),
                new PlayerChampionship("196#10#4#17"),
                new PlayerChampionship("197#10#5#5"),
                new PlayerChampionship("198#10#6#9"),
                new PlayerChampionship("199#10#7#33"),
                new PlayerChampionship("200#10#8#17"),
                new PlayerChampionship("201#10#9#33"),
                new PlayerChampionship("202#10#10#9"),
                new PlayerChampionship("203#10#11#9"),
                new PlayerChampionship("204#10#12#17"),
                new PlayerChampionship("205#10#13#5"),
                new PlayerChampionship("206#10#14#5"),
                new PlayerChampionship("207#10#15#9"),
                new PlayerChampionship("208#10#16#9"),
                new PlayerChampionship("209#10#17#5"),
                new PlayerChampionship("210#10#18#5"),
                new PlayerChampionship("211#10#19#17"),
                new PlayerChampionship("212#10#21#17"),
                new PlayerChampionship("213#10#22#9"),
                //Chris Dobey
                new PlayerChampionship("214#11#1#17"),
                new PlayerChampionship("215#11#2#9"),
                new PlayerChampionship("216#11#3#17"),
                new PlayerChampionship("217#11#6#17"),
                new PlayerChampionship("218#11#7#33"),
                new PlayerChampionship("219#11#8#9"),
                new PlayerChampionship("220#11#9#33"),
                new PlayerChampionship("221#11#10#17"),
                new PlayerChampionship("222#11#11#5"),
                new PlayerChampionship("223#11#12#5"),
                new PlayerChampionship("224#11#14#17"),
                new PlayerChampionship("225#11#15#5"),
                new PlayerChampionship("226#11#16#17"),
                new PlayerChampionship("227#11#17#5"),
                new PlayerChampionship("228#11#18#5"),
                new PlayerChampionship("229#11#19#5"),
                new PlayerChampionship("230#11#20#9"),
                new PlayerChampionship("231#11#21#17"),
                new PlayerChampionship("232#11#22#5"),
                //Gary Anderson
                new PlayerChampionship("233#12#1#2"),
                new PlayerChampionship("234#12#2#33"),
                new PlayerChampionship("235#12#3#9"),
                new PlayerChampionship("236#12#4#17"),
                new PlayerChampionship("237#12#6#9"),
                new PlayerChampionship("238#12#7#9"),
                new PlayerChampionship("239#12#8#5"),
                new PlayerChampionship("240#12#9#33"),
                new PlayerChampionship("241#12#10#17"),
                new PlayerChampionship("242#12#11#17"),
                new PlayerChampionship("243#12#14#17"),
                new PlayerChampionship("244#12#15#17"),
                new PlayerChampionship("245#12#16#9"),
                new PlayerChampionship("246#12#17#9"),
                new PlayerChampionship("247#12#18#9"),
                new PlayerChampionship("248#12#20#5"),
                new PlayerChampionship("249#12#21#17"),
                new PlayerChampionship("250#12#22#9"),
                //Gabriel Clemens
                new PlayerChampionship("251#13#1#9"),
                new PlayerChampionship("252#13#2#9"),
                new PlayerChampionship("253#13#3#17"),
                new PlayerChampionship("254#13#4#17"),
                new PlayerChampionship("255#13#5#17"),
                new PlayerChampionship("256#13#6#17"),
                new PlayerChampionship("257#13#7#17"),
                new PlayerChampionship("258#13#8#17"),
                new PlayerChampionship("259#13#9#17"),
                new PlayerChampionship("260#13#10#17"),
                new PlayerChampionship("261#13#11#17"),
                new PlayerChampionship("262#13#12#17"),
                new PlayerChampionship("263#13#14#33"),
                new PlayerChampionship("264#13#15#5"),
                new PlayerChampionship("265#13#16#33"),
                new PlayerChampionship("266#13#17#17"),
                new PlayerChampionship("267#13#18#17"),
                new PlayerChampionship("268#13#19#17"),
                new PlayerChampionship("269#13#21#5"),
                new PlayerChampionship("270#13#22#17"),
                //Luke Littler
                new PlayerChampionship("271#14#16#33"),
                new PlayerChampionship("272#14#22#2")
            });

            // id # ChampionshipId # Place # Price
            modelBuilder.Entity<Prizes>().HasData(new Prizes[]
            {
                // 1. Championship (worlds 21)
                new Prizes("1#1#1#500000"), // 1
                new Prizes("2#1#2#200000"), // 2
                new Prizes("3#1#3#100000"), // 3-4
                new Prizes("4#1#5#50000"),  // 5-8
                new Prizes("5#1#9#35000"),  // 9-16
                new Prizes("6#1#17#25000"), // 17-32
                new Prizes("7#1#33#15000"), // 33-64
                new Prizes("8#1#65#7500"),  // 65-96

                // 2. Championship (uk 21)
                new Prizes("9#2#1#100000"), // 1
                new Prizes("10#2#2#40000"),  // 2
                new Prizes("11#2#3#20000"),  // 3-4
                new Prizes("12#2#5#12500"),  // 5-8
                new Prizes("13#2#9#7500"),   // 9-16
                new Prizes("14#2#17#4000"),  // 17-32
                new Prizes("15#2#33#2000"),  // 33-64
                new Prizes("16#2#65#1000"),  // 65-96
                new Prizes("17#2#97#0"),    // Csak a UK Open championshipekben van olyan helyezés, ahol nincs pénz nyeremény (97-160)

                // 3. Championship (matchplay 21)
                new Prizes("18#3#1#150000"), // 1
                new Prizes("19#3#2#70000"),  // 2
                new Prizes("20#3#3#50000"),  // 3-4
                new Prizes("21#3#5#25000"),  // 5-8
                new Prizes("22#3#9#15000"),  // 9-16
                new Prizes("23#3#17#10000"), // 17-32

                // 4. Championship (grand prix 21)
                new Prizes("24#4#1#110000"), // 1
                new Prizes("25#4#2#50000"),  // 2
                new Prizes("26#4#3#25000"),  // 3-4
                new Prizes("27#4#5#16000"),  // 5-8
                new Prizes("28#4#9#10000"),  // 9-16
                new Prizes("29#4#17#6000"),  // 17-32

                // 5. Championship (european 21)
                new Prizes("30#5#1#120000"), // 1
                new Prizes("31#5#2#60000"),  // 2
                new Prizes("32#5#3#32000"),  // 3-4
                new Prizes("33#5#5#20000"),  // 5-8
                new Prizes("34#5#9#10000"),  // 9-16
                new Prizes("35#5#17#6000"),  // 17-32

                // 6. Championship (grand slam 21)
                new Prizes("36#6#1#125000"), // 1
                new Prizes("37#6#2#65000"),  // 2
                new Prizes("38#6#3#40000"),  // 3-4 
                new Prizes("39#6#5#20000"),  // 5-8
                new Prizes("40#6#9#10000"),  // 9-16
                new Prizes("41#6#17#7500"),  // 17-24
                new Prizes("42#6#25#4000"),  // 25-32

                // 7. Championship (players finals 21)
                new Prizes("43#7#1#100000"), // 1
                new Prizes("44#7#2#50000"),  // 2
                new Prizes("45#7#3#25000"),  // 3-4
                new Prizes("46#7#5#15000"),  // 5-8
                new Prizes("47#7#9#10000"),  // 9-16
                new Prizes("48#7#17#5000"),  // 17-32
                new Prizes("49#7#33#2500"),  // 32-64

                // 8. Championship (worlds 22)
                new Prizes("50#8#1#500000"),
                new Prizes("51#8#2#200000"),
                new Prizes("52#8#3#100000"),
                new Prizes("53#8#5#50000"),
                new Prizes("54#8#9#35000"),
                new Prizes("55#8#17#25000"),
                new Prizes("56#8#33#15000"),
                new Prizes("57#8#65#7500"),

                // 9. Championship (uk 22)
                new Prizes("58#9#1#100000"),
                new Prizes("59#9#2#40000"),
                new Prizes("60#9#3#20000"),
                new Prizes("61#9#5#12500"),
                new Prizes("62#9#9#7500"),
                new Prizes("63#9#17#4000"),
                new Prizes("64#9#33#2000"),
                new Prizes("65#9#65#1000"),
                new Prizes("66#9#97#0"),

                // 10. Championship (matchplay 22)
                new Prizes("67#10#1#200000"),
                new Prizes("68#10#2#100000"),
                new Prizes("69#10#3#50000"),
                new Prizes("70#10#5#30000"),
                new Prizes("71#10#9#15000"),
                new Prizes("72#10#17#10000"),

                // 11. Championship (grand prix 22)
                new Prizes("73#11#1#120000"),
                new Prizes("74#11#2#60000"),
                new Prizes("75#11#3#40000"),
                new Prizes("76#11#5#25000"),
                new Prizes("77#11#9#15000"),
                new Prizes("78#11#17#7500"),

                // 12. Championship (european 22)
                new Prizes("79#12#1#120000"),
                new Prizes("80#12#2#60000"),
                new Prizes("81#12#3#32000"),
                new Prizes("82#12#5#20000"),
                new Prizes("83#12#9#10000"),
                new Prizes("84#12#17#6000"),

                // 13. Championship (grand slam 22)
                new Prizes("85#13#1#150000"),
                new Prizes("86#13#2#70000"),
                new Prizes("87#13#3#50000"), 
                new Prizes("88#13#5#25000"),
                new Prizes("89#13#9#12250"),
                new Prizes("90#13#17#8000"),
                new Prizes("91#13#25#5000"),

                // 14. Championship (players finals 22)
                new Prizes("92#14#1#100000"),
                new Prizes("93#14#2#50000"),
                new Prizes("94#14#3#25000"),
                new Prizes("95#14#5#15000"),
                new Prizes("96#14#9#10000"),
                new Prizes("97#14#17#5000"),
                new Prizes("98#14#33#2500"),

                // 15. Championship (worlds 23)
                new Prizes("99#15#1#500000"),
                new Prizes("100#15#2#200000"),
                new Prizes("101#15#3#100000"),
                new Prizes("102#15#5#50000"),
                new Prizes("103#15#9#35000"),
                new Prizes("104#15#17#25000"),
                new Prizes("105#15#33#15000"),
                new Prizes("106#15#65#7500"),

                // 16. Championship (uk 23)
                new Prizes("107#16#1#110000"),
                new Prizes("108#16#2#50000"),
                new Prizes("109#16#3#30000"),
                new Prizes("110#16#5#15000"),
                new Prizes("111#16#9#10000"),
                new Prizes("112#16#17#5000"),
                new Prizes("113#16#33#2500"),
                new Prizes("114#16#65#1500"),
                new Prizes("115#16#97#1000"),
                new Prizes("116#16#129#0"),

                // 17. Championship (matchplay 23)
                new Prizes("117#17#1#200000"),
                new Prizes("118#17#2#100000"),
                new Prizes("119#17#3#50000"),
                new Prizes("120#17#5#30000"),
                new Prizes("121#17#9#15000"),
                new Prizes("122#17#17#10000"),

                // 18. Championship (grand prix 23)
                new Prizes("123#18#1#120000"),
                new Prizes("124#18#2#60000"),
                new Prizes("125#18#3#40000"),
                new Prizes("126#18#5#25000"),
                new Prizes("127#18#9#15000"),
                new Prizes("128#18#17#7500"),

                // 19. Championship (european 23)
                new Prizes("129#19#1#120000"),
                new Prizes("130#19#2#60000"),
                new Prizes("131#19#3#40000"),
                new Prizes("132#19#5#25000"),
                new Prizes("133#19#9#15000"),
                new Prizes("134#19#17#7500"),

                // 20. Championship (grand slam 23)
                new Prizes("135#20#1#150000"),
                new Prizes("136#20#2#70000"),
                new Prizes("137#20#3#50000"),
                new Prizes("138#20#5#25000"),
                new Prizes("139#20#9#12250"),
                new Prizes("140#20#17#8000"),
                new Prizes("141#20#25#5000"),

                // 21. Championship (players finals 23)
                new Prizes("142#21#1#120000"),
                new Prizes("143#21#2#60000"),
                new Prizes("144#21#3#30000"),
                new Prizes("145#21#5#20000"),
                new Prizes("146#21#9#10000"),
                new Prizes("147#21#17#6500"),
                new Prizes("148#21#33#3000"),

                // 22. Championship (worlds 24)
                new Prizes("149#22#1#500000"),
                new Prizes("150#22#2#200000"),
                new Prizes("151#22#3#100000"),
                new Prizes("152#22#5#50000"),
                new Prizes("153#22#9#35000"),
                new Prizes("154#22#17#25000"),
                new Prizes("155#22#33#15000"),
                new Prizes("156#22#65#7500")
            });
            
        }
    }
}
