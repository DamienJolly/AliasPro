using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Settings;
using AliasPro.Catalog;
using AliasPro.Chat;
using AliasPro.Configuration;
using AliasPro.Figure;
using AliasPro.Items;
using AliasPro.Landing;
using AliasPro.Messenger;
using AliasPro.Navigator;
using AliasPro.Network;
using AliasPro.Network.Events;
using AliasPro.Players;
using AliasPro.Rooms;
using AliasPro.Sessions;
using AliasPro.Settings;
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
                new NetworkService(),
                new SettingsService(),
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
                new ConfigurationService()
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

            ISettingsController settings = _serviceProvider.GetService<ISettingsController>();
            await settings.CleanupDatabase();

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    string input = Console.ReadLine().ToLower();
                    //todo: console commands.
                }
            }
        }
        
        static Task Main() => new Program().Run();
    }
}
