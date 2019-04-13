using AliasPro.API.Navigator;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Navigator.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Navigator
{
    internal class NavigatorService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<NavigatorDao>();
            collection.AddSingleton<NavigatorRepository>();
            collection.AddSingleton<INavigatorController, NavigatorController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, InitializeNavigatorEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserFlatCatsEvent>();
            collection.AddSingleton<IAsyncPacket, RequestNavigatorFlatsEvent>();
            collection.AddSingleton<IAsyncPacket, NavigatorSearchEvent>();
            collection.AddSingleton<IAsyncPacket, UpdateNavigatorPreferencesEvent>();
        }
    }
}
