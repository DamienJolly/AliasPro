using AliasPro.Achievements;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Server;
using AliasPro.Badges;
using AliasPro.Catalog;
using AliasPro.Chat;
using AliasPro.Configuration;
using AliasPro.Figure;
using AliasPro.Groups;
using AliasPro.Items;
using AliasPro.Landing;
using AliasPro.Logging;
using AliasPro.Messenger;
using AliasPro.Moderation;
using AliasPro.Navigator;
using AliasPro.Network;
using AliasPro.Network.Events;
using AliasPro.Permissions;
using AliasPro.Pets;
using AliasPro.Players;
using AliasPro.Rooms;
using AliasPro.Server;
using AliasPro.Sessions;
using AliasPro.Trading;
using AliasPro.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AliasPro
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

		private Program()
        {
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

            IList<INetworkService> services = new List<INetworkService>
			{
				new ServerService(),
				new NetworkService(),
				new ModerationService(),
				new MessengerService(),
				new PlayerService(),
				new SessionService(),
				new LandingService(),
				new RoomService(),
				new NavigatorService(),
				new ItemService(),
				new CatalogService(),
				new ChatService(),
				new FigureService(),
				new ConfigurationService(),
				new PermissionsService(),
				new AchievementService(),
				new BadgeService(),
				new GroupService(),
				new TradingService(),
				new PetService()
			};

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logging =>
            {
                logging.AddProvider(new CustomLoggerProvider());
            });

            foreach (INetworkService service in services)
            {
                service.SetupService(serviceCollection);
            }
            
            serviceCollection.AddSingleton<IEventProvider, EventProvider>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private async Task Run()
        {
            INetworkListener listener = _serviceProvider.GetService<INetworkListener>();
            await listener.Listen(30000);

            IServerController server = _serviceProvider.GetService<IServerController>();
            await server.CleanupDatabase();

            Console.Title = "Alias Emulator is online!";

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    string input = Console.ReadLine().ToLower();
                }
            }
        }
        
        static Task Main() => new Program().Run();
    }
}