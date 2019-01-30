using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AliasPro.Network;
using AliasPro.Network.Events;
using AliasPro.Sessions;
using AliasPro.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                new SessionService()
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
