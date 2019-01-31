using AliasPro.Network;
using AliasPro.Network.Events;
using AliasPro.Player.Packets.Incoming;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Player
{
    internal class PlayerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, UniqueIdEvent>();
            collection.AddSingleton<IAsyncPacket, SecureLoginEvent>();
        }
    }
}
