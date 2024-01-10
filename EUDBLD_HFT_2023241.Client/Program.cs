using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters;
using ConsoleTools;
using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using EUDBLD_HFT_2023241.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Channels;

namespace EUDBLD_HFT_2023241.Client
{
    // !! Kivenni minden függőséget REST API -nál, kiéve MODELS-t
    // !! Startup -ot visszaállítani !!

    internal class Program
    {
        static PlayerLogic playerLogic;
        static PlayerChampionshipLogic playerChampionshipLogic;
        static ChampionshipLogic championshipLogic;
        static PrizeLogic prizeLogic;

        static ConsoleMenu baseMenu;

        static void Create(string entity)
        {
            if(entity == "Players")
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Create Player");
                    Console.Write("Player name: ");
                    playerLogic.Create(new Player() { Name = Console.ReadLine() });
                    Console.WriteLine("Player Created!");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            if(entity == "Championships")
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Create Championship");
                    Championship newCh = new Championship()
                    {
                        Name = GetStringFromUser("Name: "),
                        MaxAttender = GetIntFromUser("Size: "),
                        PrizePool = GetIntFromUser("Prize pool: "),
                        StartDate = DateTime.Parse(GetStringFromUser("Start Date: ")),
                        EndDate = DateTime.Parse(GetStringFromUser("Start End: "))
                    };

                    championshipLogic.Create(newCh);
                    Console.WriteLine("Championship Created!");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            Console.ReadLine();
        }

        #region List
        static void List(string entity)
        {
            if (entity == "Player Ranks")
            {
                var playersInOrder = playerLogic.GetPlayersInOrder();
                int i = 0;
                foreach (var item in playersInOrder)
                {
                    Console.WriteLine($"Rank {++i}: {item.P.Name}\t\t({playerLogic.PlayersRankingMoney(item.P.Id)} Pounds)");
                }
                Console.ReadLine();
            }
            if (entity == "Custom Player Ranks")
            {
                try
                {
                    DateTime customDate = DateTime.Parse(GetStringFromUser("Write a date: "));
                    var playersInOrder = playerLogic.GetPlayersInOrder(customDate);
                    int i = 0;
                    foreach (var item in playersInOrder)
                    {
                        Console.WriteLine($"Rank {++i}: {item.P.Name}\t\t({playerLogic.PlayersRankingMoney(item.P.Id)} Pounds)");
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.ReadLine();
            }
        }
        
        static void SelectableList(string entity)
        {
            if(entity == "Players")
            {
                var playerListSubMenu = new ConsoleMenu();
                foreach (var item in playerLogic.ReadAll())
                {
                    playerListSubMenu.Add(item.Name, (thisMenu) => { thisMenu.CloseMenu(); ItemSelected(item); });
                }
                playerListSubMenu.Add("Exit", ConsoleMenu.Close);
                playerListSubMenu.Show();
            }
            if(entity == "Championships")
            {
                var championshipListSubMenu = new ConsoleMenu();
                foreach (var item in championshipLogic.ReadAll())
                {
                    championshipListSubMenu.Add(item.Name, (thisMenu) => { thisMenu.CloseMenu(); ItemSelected(item); });
                }
                championshipListSubMenu.Add("Exit", ConsoleMenu.Close);
                championshipListSubMenu.Show();
            }
        }

        // if an entity gets selected do...
        static void ItemSelected(Entity e)
        {
            if (e is Championship)
            {
                Championship current = (Championship)e;
                string title = "";
                title += "Name: " + current.Name + "\n";
                title += $"Date: {current.StartDate.ToString("yyyy.MM.dd.")} - {current.EndDate.ToString("yyyy.MM.dd.")}\n";
                title += "Size: " + current.MaxAttender + " players \n";
                title += "Prize pool: " + current.PrizePool + "\n";


                var championshipAssignSubMenu = new ConsoleMenu()
                .Add("TOP 4", () => ShowResult("Top4", e))
                .Add("Results", () => ShowResult("Championship Results", e, title))
                .Add("Update", (thisMenu) => { thisMenu.CloseMenu(); ShowResult("Championship Update", e, title); })
                .Add("Manage Players", () => ShowResult("Manage Players", e, title))
                .Add("Manage Prizes", () => ShowResult("Manage Prizes", e, title))
                .Add("Delete", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Delete Championship", e, title); })
                .Add("Exit", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Title = title;
                    config.EnableWriteTitle = true;
                });
                championshipAssignSubMenu.Show();
            }
            if (e is Player)
            {
                Player current = (Player)e;
                string title = "";
                title += "Name: " + current.Name + "\n";
                title += "Current Rank: " + playerLogic.GetPlayersRank(current) + "\n";
                title += "Current winnings: " + playerLogic.PlayersRankingMoney(current.Id) + " Pounds \n";

                var championshipAssignSubMenu = new ConsoleMenu()
                .Add("Attended Championships", () => ShowResult("Attended Championships", e, title))
                .Add("Update Player", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Player Update", e, title); })
                .Add("Delete Player", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Player Delete", e, title); })
                .Add("Exit", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Title = title;
                    config.EnableWriteTitle = true;
                });
                championshipAssignSubMenu.Show();
            }
        }

        static void ShowResult(string function, Entity e, string title = "")
        {
            if (function == "Top4")
            {
                try
                {
                    List<Player> topPlayers = playerLogic.GetTopPlayersFromChampionship(e.Id, 4).ToList();
                    for (int i = 0; i < topPlayers.Count; i++)
                    {
                        Console.WriteLine($"{playerLogic.GetPlayersPlaceInChampionship(topPlayers[i].Id, e.Id)}.Place: {topPlayers[i].Name}");
                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
            if (function == "Championship Results")
            {
                try
                {
                    Championship current = (Championship)e;
                    Championship result = championshipLogic.Read(current.Id);
                    List<Player> participants = playerLogic.GetTopPlayersFromChampionship(current.Id, current.Attenders.Count()).ToList();
                    foreach (var item in participants)
                    {
                        Console.WriteLine(playerLogic.GetPlayersPlaceInChampionship(item.Id, current.Id) + ".Place: " + item.Name);
                    }
                }
                catch (Exception exc) 
                {
                    Console.WriteLine(exc.Message);
                }    
            }
            if (function == "Championship Update")
            {
                var championshipUpdateMenu = new ConsoleMenu()
                    .Add("Name", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Update Championship Name", e, title); })
                    .Add("Size", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Update Championship Size", e, title); })
                    .Add("Prize pool", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Update Championship Prize pool", e, title); })
                    .Add("Start Date", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Update Championship Start Date", e, title); })
                    .Add("End Date", (thisMenu) => { thisMenu.CloseMenu(); RecordHandler("Update Championship End Date", e, title); })
                    .Add("Exit", ConsoleMenu.Close)
                    .Configure(config =>
                    {
                        config.Title = title;
                        config.EnableWriteTitle = true;
                    });
                    championshipUpdateMenu.Show();
            }
            if (function == "Manage Players")
            {
                var playersManageSubMenu = new ConsoleMenu()
                .Add("Add Player to the championship", () => RecordHandler("Add Player", e, title))
                .Add("Update Player's place in the championship", () => RecordHandler("Update Player place", e, title))
                .Add("Remove Player from the championship", () => RecordHandler("Remove Player", e, title))
                .Add("Exit", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Title = title;
                    config.EnableWriteTitle = true;
                });
                playersManageSubMenu.Show();
            }
            if (function == "Manage Prizes")
            {
                var prizeManageSubMenu = new ConsoleMenu()
                .Add("Add a prize to the championship", () => RecordHandler("Add Prize", e, title))
                .Add("Update a prize in the championship", () => RecordHandler("Update Prize", e, title))
                .Add("Remove a prize from the championship", () => RecordHandler("Delete Prize", e, title))
                .Add("Exit", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Title = title;
                    config.EnableWriteTitle = true;
                });
                prizeManageSubMenu.Show();
            }
            if (function == "Attended Championships")
            {
                Player current = (Player)e;
                List<Championship> participatedChampionships = playerLogic.GetAttendedChampionships(current.Id).ToList();
                List<Championship> rankedParticipatedChampionships = playerLogic.PlayersRankingAttandences(current.Id).ToList();

                List<Championship> sameChampships = participatedChampionships.Intersect(rankedParticipatedChampionships).ToList();

                Console.Clear();
                Console.WriteLine(current.Name + "\n");
                foreach (var item in participatedChampionships)
                {
                    if (sameChampships.Contains(item))
                        Console.Write("* ");
                    string name = item.Name;
                    int place = playerLogic.GetPlayersPlaceInChampionship(current.Id, item.Id);
                    int prize = 0;
                    // Ha nincs beállítva prize a helyezésének, akkor 0 marad a nyeremény
                    try { prize = playerLogic.GetPlayersPrizeForChampionship(current.Id, item); } catch {}
                    Console.WriteLine($"{name}:\t {place}.Place ({prize} pounds)");
                }
            }
            Console.ReadLine();
        }

        static void RecordHandler(string entity, Entity e, string title = "")
        {
            if (e is Championship)
            {
                Championship current = (Championship)e;

                // Championship CRUD
                if (entity == "Add Player")
                {
                    try
                    {
                        var managedPlayerSubMenu = new ConsoleMenu();
                        foreach (var item in playerLogic.GetChampionshipMissingPlayers(current))
                        {
                            managedPlayerSubMenu.Add(item.Name, (thisMenu) =>
                            {
                                playerChampionshipLogic.Create(new PlayerChampionship() { ChampionshipId = e.Id, PlayerId = item.Id, Place = GetIntFromUser("What place did this player finished at?: ") });
                                thisMenu.CloseMenu();
                                Console.WriteLine("Player successfully added to the championship!");
                            });
                        }
                        managedPlayerSubMenu.Add("Exit", ConsoleMenu.Close);
                        managedPlayerSubMenu.Configure(config =>
                        {
                            config.Title = title;
                            config.EnableWriteTitle = true;
                        });
                        managedPlayerSubMenu.Show();
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Player place")
                {
                    try
                    {
                        var managedPlayerSubMenu = new ConsoleMenu();
                        foreach (var item in current.Attenders)
                        {
                            managedPlayerSubMenu.Add($"{item.Name} ({playerLogic.GetPlayersPlaceInChampionship(item.Id, current.Id)})", (thisMenu) =>
                            {
                                var old = playerChampionshipLogic.Read(playerChampionshipLogic.GetId(item.Id, current.Id));
                                old.Place = GetIntFromUser("What place did this player finished at?: ");
                                playerChampionshipLogic.Update(old);
                                thisMenu.CloseMenu();
                                Console.WriteLine("Player's palce was successfully modified!");
                            });
                        }
                        managedPlayerSubMenu.Add("Exit", ConsoleMenu.Close);
                        managedPlayerSubMenu.Configure(config =>
                        {
                            config.Title = title;
                            config.EnableWriteTitle = true;
                        });
                        managedPlayerSubMenu.Show();
                    }
                    catch(Exception exc)
                    {
                        Console.Clear();
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Remove Player")
                {
                    var managedPlayerSubMenu = new ConsoleMenu();
                    foreach (var item in current.Attenders)
                    {
                        managedPlayerSubMenu.Add(item.Name, (thisMenu) =>
                        {
                            championshipLogic.DeletePlayerFromChampionship(item.Id, current.Id);
                            thisMenu.CloseMenu();
                            Console.WriteLine("Player successfully removed from the championship!");
                        });
                    }
                    managedPlayerSubMenu.Add("Exit", ConsoleMenu.Close);
                    managedPlayerSubMenu.Configure(config =>
                    {
                        config.Title = title;
                        config.EnableWriteTitle = true;
                    });
                    managedPlayerSubMenu.Show();
                    
                }
                if (entity == "Update Championship Name")
                {
                    try
                    {
                        current.Name = GetStringFromUser("New name: ");
                        championshipLogic.Update(current);
                        Console.WriteLine("Name of the championships has successfully changed!");
                    }
                    catch(Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Championship Size")
                {
                    try
                    {
                        current.MaxAttender = GetIntFromUser("New Size: ");
                        championshipLogic.Update(current);
                        Console.WriteLine("New size of the championships has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Championship Prize pool")
                {
                    try
                    {
                        current.PrizePool = GetIntFromUser("New Prize pool: ");
                        championshipLogic.Update(current);
                        Console.WriteLine("Prize pool of the championships has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Championship Start Date")
                {
                    try
                    {
                        current.StartDate = DateTime.Parse(GetStringFromUser("New Start Date: "));
                        championshipLogic.Update(current);
                        Console.WriteLine("Start date of the championships has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Championship End Date")
                {
                    try
                    {
                        current.EndDate = DateTime.Parse(GetStringFromUser("New End Date: "));
                        championshipLogic.Update(current);
                        Console.WriteLine("End date of the championships has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Delete Championship")
                {
                    championshipLogic.Delete(e.Id);
                    Console.WriteLine("Championship deleted Successfully!");
                }
                
                // Prize CRUD
                if (entity == "Add Prize")
                {
                    try
                    {
                        prizeLogic.Create(new Prizes() { ChampionshipId = current.Id, Place = GetIntFromUser("Place: "), Price = GetIntFromUser("Prize: ") });
                        Console.WriteLine("New Prize added Successfully!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Update Prize")
                {
                    try
                    {
                        var managedPrizeSubMenu = new ConsoleMenu();
                        foreach (var item in prizeLogic.GetAllPrizesInChampionship(current.Id))
                        {
                            managedPrizeSubMenu.Add($"{item.Place}: {item.Price} pounds", (thisMenu) =>
                            {
                                item.Place = GetIntFromUser("New Place: ");
                                item.Price = GetIntFromUser("New Prize: ");
                                prizeLogic.Update(item);
                                thisMenu.CloseMenu();
                                Console.WriteLine("The Prize was successfully modified!");
                            });
                        }
                        managedPrizeSubMenu.Add("Exit", ConsoleMenu.Close);
                        managedPrizeSubMenu.Configure(config =>
                        {
                            config.Title = title;
                            config.EnableWriteTitle = true;
                        });
                        managedPrizeSubMenu.Show();
                    }
                    catch (Exception exc)
                    {
                        Console.Clear();
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Delete Prize")
                {
                    var managedPrizeSubMenu = new ConsoleMenu();
                    foreach (var item in prizeLogic.GetAllPrizesInChampionship(current.Id))
                    {
                        managedPrizeSubMenu.Add($"{item.Place}: {item.Price} pounds", (thisMenu) =>
                        {
                            prizeLogic.Delete(item.Id);
                            thisMenu.CloseMenu();
                            Console.WriteLine("Prize successfully removed from the championship!");
                        });
                    }
                    managedPrizeSubMenu.Add("Exit", ConsoleMenu.Close);
                    managedPrizeSubMenu.Configure(config =>
                    {
                        config.Title = title;
                        config.EnableWriteTitle = true;
                    });
                    managedPrizeSubMenu.Show();
                }   
            }
            if (e is Player)
            {
                Player current = (Player)e;
                
                if (entity == "Player Update")
                {
                    try
                    {
                        current.Name = GetStringFromUser("New name: ");
                        playerLogic.Update(current);
                        Console.WriteLine("Name has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Player Delete")
                {
                    playerLogic.Delete(e.Id);
                    Console.WriteLine("Player deleted Successfully!");
                }
            }
            Console.ReadLine();
        }
        #endregion List  

        static int GetIntFromUser(string text)
        {
            Console.Write(text);
            return int.Parse(Console.ReadLine());
        }

        static string GetStringFromUser(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }
 

        static void Main(string[] args)
        {
            var ctx = new DartsDbContext();

            var playerRepo = new PlayerRepository(ctx);
            var championshipRepo = new ChampionshipRepository(ctx);
            var prizesRepo = new PrizesRepository(ctx);
            var playerChampionshipRepo = new PlayerChampionshipRepository(ctx);

            playerLogic = new PlayerLogic(playerRepo, playerChampionshipRepo);
            championshipLogic = new ChampionshipLogic(championshipRepo, playerChampionshipRepo);
            prizeLogic = new PrizeLogic(prizesRepo, championshipRepo);
            playerChampionshipLogic = new PlayerChampionshipLogic(playerChampionshipRepo, championshipRepo);

            var playerSubmenu = new ConsoleMenu(args, level: 1)
                .Add("Create Player", () => Create("Players"))
                .Add("List Players", () => SelectableList("Players"))
                .Add("List Current Player Rankings", () => List("Player Ranks"))
                .Add("List Custom Player Rankings", () => List("Custom Player Ranks"))
                .Add("Exit", ConsoleMenu.Close);


            var championshipSubmenu = new ConsoleMenu(args, level: 1)
                .Add("Create Championship", () => Create("Championships"))
                .Add("List Championships", () => SelectableList("Championships"))
                .Add("Exit", ConsoleMenu.Close);


            baseMenu = new ConsoleMenu(args, level: 0)
                .Add("Player", () => playerSubmenu.Show())
                .Add("Championship", () => championshipSubmenu.Show())
                .Add("Exit", () => Environment.Exit(0));

            baseMenu.Show();
        }
    }
}
