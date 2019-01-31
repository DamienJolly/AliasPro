using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AliasPro
{
    using Landing;
    using Network;
    using Network.Events;
    using Player;
    using Sessions;

    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

        private Program()
        {
            IList<INetworkService> services = new List<INetworkService>
            {
                new NetworkService(),
                new PlayerService(),
                new SessionService(),
                new LandingService()
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

            Console.ReadKey();
        }
        
        static Task Main() => new Program().Run();
    }
}
