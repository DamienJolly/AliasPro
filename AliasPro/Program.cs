﻿using AliasPro.API.Groups;
using AliasPro.API.Rooms;
using AliasPro.API.Server;
using AliasPro.API.Sessions;
using AliasPro.Communication.Messages;
using AliasPro.Configuration;
using AliasPro.Crafting;
using AliasPro.Currency;
using AliasPro.Figure;
using AliasPro.Game.Achievements;
using AliasPro.Game.Badges;
using AliasPro.Game.Catalog;
using AliasPro.Game.Chat;
using AliasPro.Groups;
using AliasPro.Items;
using AliasPro.Landing;
using AliasPro.Logging;
using AliasPro.Messenger;
using AliasPro.Moderation;
using AliasPro.Navigator;
using AliasPro.Network.Game;
using AliasPro.Permissions;
using AliasPro.Pets;
using AliasPro.Players;
using AliasPro.Rooms;
using AliasPro.Server;
using AliasPro.Sessions;
using AliasPro.Trading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        public static IServerController Server;

        private Program()
        {
            Console.Clear();
            Console.Title = "Alias Emulator is starting up...";
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                      ______   _      _");
            Console.WriteLine("                     |  __  | | |    |_|  ______    ______");
            Console.WriteLine("                     | |__| | | |     _  |  __  |  |  ____|");
            Console.WriteLine("                     |  __  | | |__  | | | |__| |  |____  |");
            Console.WriteLine(@"                     |_|  |_| |____| |_| |_______\ |______|");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("                                   Build: " + "2.0");
            Console.WriteLine("                             https://DamienJolly.com");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;

            IList<IService> services = new List<IService>
			{
				new ServerService(),
				new ModerationService(),
				new MessengerService(),
				new PlayerService(),
				new SessionService(),
				new LandingService(),
				new RoomService(),
				new NavigatorService(),
				new ItemService(),
				new FigureService(),
				new ConfigurationService(),
				new PermissionsService(),
				new GroupService(),
				new TradingService(),
				new PetService(),
				new CraftingService(),
				new CurrencyService(),

                new BadgeService(),
                new AchievementService(),
                new CatalogService(),
                new ChatService(),
            };

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logging =>
            {
                logging.AddProvider(new CustomLoggerProvider());
            });

            foreach (IService service in services)
            {
                service.Register(serviceCollection);
            }
            
            serviceCollection.AddSingleton<MessageHandler>();
            serviceCollection.AddSingleton<GameNetworkListener>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static T GetService<T>() =>
            _serviceProvider.GetService<T>();

        private async Task Run()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            GameNetworkListener listener = GetService<GameNetworkListener>();
            await listener.StartAsync(30000);

            IServerController server = GetService<IServerController>();
            await server.CleanupDatabase();
            Server = server;

            IGroupController group = GetService<IGroupController>();
            group.InitalizeBadgeImager();

            Console.Title = "Alias Emulator is online!";

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.Write("[Alias] [Command] : ");
                    string input = Console.ReadLine().ToLower();

                    if (input == "shutdown")
                    {
                        ISessionController sessionController = GetService<ISessionController>();
                        IRoomController roomController = GetService<IRoomController>();
                        foreach (var room in roomController.Rooms.ToList())
                        {
                            roomController.DisposeRoom(room);
                        }
                        foreach (var session in sessionController.Sessions)
                        {
                            session.Disconnect();
                        }

                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            Console.WriteLine("crash");
            //Logger.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e);
            //shutdown
        }

        static Task Main() => new Program().Run();
    }
}