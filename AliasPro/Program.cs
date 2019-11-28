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
using AliasPro.Messenger;
using AliasPro.Moderation;
using AliasPro.Navigator;
using AliasPro.Network;
using AliasPro.Network.Events;
using AliasPro.Permissions;
using AliasPro.Players;
using AliasPro.Rooms;
using AliasPro.Server;
using AliasPro.Sessions;
using AliasPro.Trading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

		private Program()
        {
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
				new TradingService()
			};

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logging =>
            {
                logging.AddConsole();
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
