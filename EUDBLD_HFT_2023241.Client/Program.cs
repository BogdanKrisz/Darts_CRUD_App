using ConsoleTools;
using EUDBLD_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Channels;

namespace EUDBLD_HFT_2023241.Client
{
    internal class Program
    {
        static RestService rest;

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
                    Player newPlayer = new Player() { Name = Console.ReadLine() };
                    rest.Post(newPlayer, "Player");
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

                    rest.Post(newCh, "Championship");
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
                try
                {
                    List<Player> playersInOrder = rest.Get<Player>("Stat/GetPlayersInOrder");
                    int i = 0;
                    foreach (var item in playersInOrder)
                    {
                        int rankingMoney = rest.GetSingle<int>($"Stat/PlayersRankingMoney?playerId={item.Id}");
                        Console.WriteLine($"Rank {++i}: {item.Name}\t\t({rankingMoney} Pounds)");
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.ReadLine();
            }
            if (entity == "Custom Player Ranks")
            {
                try
                {
                    DateTime customDate = DateTime.Parse(GetStringFromUser("Write a date: "));

                    var playersInOrder = rest.Get<Player>($"Stat/GetPlayersInOrder/{customDate}");
                    int i = 0;
                    foreach (var item in playersInOrder)
                    {
                        // Itt időt bekéne adni
                        int rankingMoney = rest.GetSingle<int>($"Stat/PlayersRankingMoney/{customDate}?playerId={item.Id}");
                        Console.WriteLine($"Rank {++i}: {item.Name}\t\t({rankingMoney} Pounds)");
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
                var allPlayer = rest.Get<Player>("Player");
                foreach (var item in allPlayer)
                {
                    playerListSubMenu.Add(item.Name, (thisMenu) => { thisMenu.CloseMenu(); ItemSelected(item); });
                }
                playerListSubMenu.Add("Exit", ConsoleMenu.Close);
                playerListSubMenu.Show();
            }
            if(entity == "Championships")
            {
                var championshipListSubMenu = new ConsoleMenu();
                var allChampships = rest.Get<Championship>("Championship");
                foreach (var item in allChampships)
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
                title += "Current Rank: " + rest.GetSingle<int>($"Stat/GetPlayersRank?playerId={current.Id}") + "\n";
                title += "Current winnings: " + rest.GetSingle<int>($"Stat/PlayersRankingMoney?playerId={current.Id}") + " Pounds \n";

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
                    int numOfPlayers = 4;
                    List<Player> topPlayers = rest.Get<Player>($"Stat/GetTopPlayersFromChampionship?championshipId={e.Id}&numberOfPlayers={numOfPlayers}");
                    for (int i = 0; i < topPlayers.Count; i++)
                    {
                        int place = rest.GetSingle<int>($"Stat/GetPlayersPlaceInChampionship?playerId={topPlayers[i].Id}&championshipId={e.Id}");
                        Console.WriteLine($"{place}.Place: {topPlayers[i].Name}");
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
                    List<Player> participants = rest.Get<Player>($"Stat/GetTopPlayersFromChampionship?championshipId={current.Id}&numberOfPlayers={current.Attenders.Count()}");
                    for (int i = 0; i < participants.Count(); i++)
                    {
                        int place = rest.GetSingle<int>($"Stat/GetPlayersPlaceInChampionship?playerId={participants[i].Id}&championshipId={e.Id}");
                        Console.WriteLine($"{place}.Place: {participants[i].Name}");
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
                List<Championship> participatedChampionships = rest.Get<Championship>($"Stat/GetAttendedChampionships?playerId={current.Id}");
                List<Championship> rankedParticipatedChampionships = rest.Get<Championship>($"Stat/PlayersRankingAttandences?playerId={current.Id}");

                List<Championship> sameChampships = participatedChampionships.Intersect(rankedParticipatedChampionships).ToList();

                Console.Clear();
                Console.WriteLine(current.Name + "\n");
                foreach (var item in participatedChampionships)
                {
                    if (sameChampships.Contains(item))
                        Console.Write("* ");
                    string name = item.Name;
                    int place = rest.GetSingle<int>($"Stat/GetPlayersPlaceInChampionship?playerId={current.Id}&championshipId={item.Id}");
                    int prize = 0;
                    try { prize = rest.GetSingle<int>($"Stat/GetPlayersPrizeForChampionship?playerId={current.Id}&champshipId={item.Id}"); } catch {}
                    Console.WriteLine($"{name}:\t {place}.Place ({prize} pounds)");
                }
                if(participatedChampionships.Count == 0)
                    Console.WriteLine("This player hasn't participate in any championship so far!");
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
                        var missingPlayers = rest.Get<Player>($"Stat/GetChampionshipMissingPlayers?championshipId={current.Id}");
                        foreach (var item in missingPlayers)
                        {
                            managedPlayerSubMenu.Add(item.Name, (thisMenu) =>
                            {
                                var newPlayerChampionship = new PlayerChampionship() { ChampionshipId = e.Id, PlayerId = item.Id, Place = GetIntFromUser("What place did this player finished at?: ") };
                                rest.Post(newPlayerChampionship, "PlayerChampionship");
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
                            managedPlayerSubMenu.Add($"{item.Name} ({rest.GetSingle<int>($"Stat/GetPlayersPlaceInChampionship?playerId={item.Id}&championshipId={current.Id}")})", (thisMenu) =>
                            {
                                int oldId = rest.GetSingle<int>($"Stat/GetId?playerId={item.Id}&ChampionshipId={current.Id}");
                                var old = rest.Get<PlayerChampionship>(oldId, "PlayerChampionship");
                                old.Place = GetIntFromUser("What place did this player finished at?: ");
                                rest.Put(old, $"PlayerChampionship/{old.Id}");
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
                            int pcId = rest.GetSingle<int>($"Stat/GetId?playerId={item.Id}&ChampionshipId={current.Id}");
                            rest.Delete(pcId, "PlayerChampionship");
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
                        rest.Put(current, $"Championship/{current.Id}");
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
                        rest.Put(current, $"Championship/{current.Id}");
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
                        rest.Put(current, $"Championship/{current.Id}");
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
                        rest.Put(current, $"Championship/{current.Id}");
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
                        rest.Put(current, $"Championship/{current.Id}");
                        Console.WriteLine("End date of the championships has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Delete Championship")
                {
                    rest.Delete(e.Id, "Championship");
                    Console.WriteLine("Championship deleted Successfully!");
                }
                
                // Prize CRUD
                if (entity == "Add Prize")
                {
                    try
                    {
                        var newPrize = new Prizes() { ChampionshipId = current.Id, Place = GetIntFromUser("Place: "), Price = GetIntFromUser("Prize: ") };
                        rest.Post(newPrize, "Prizes");
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
                        var allPrizesInChampship = rest.Get<Prizes>($"Stat/GetAllPrizesInChampionship?championshipId={current.Id}");
                        foreach (var item in allPrizesInChampship)
                        {
                            managedPrizeSubMenu.Add($"{item.Place}: {item.Price} pounds", (thisMenu) =>
                            {
                                item.Place = GetIntFromUser("New Place: ");
                                item.Price = GetIntFromUser("New Prize: ");
                                rest.Put(item.Id, $"Prizes/{item.Id}");
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
                    var allPrizesInChampship = rest.Get<Prizes>($"Stat/GetAllPrizesInChampionship?championshipId={current.Id}");
                    foreach (var item in allPrizesInChampship)
                    {
                        managedPrizeSubMenu.Add($"{item.Place}: {item.Price} pounds", (thisMenu) =>
                        {
                            rest.Delete(item.Id, "Prizes");
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
                        rest.Put(current, $"Player/{current.Id}");
                        Console.WriteLine("Name has successfully changed!");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                if (entity == "Player Delete")
                {
                    rest.Delete(e.Id, "Player");
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
            rest = new RestService("http://localhost:44758/", "Player");

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
