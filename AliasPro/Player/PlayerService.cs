using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Player
{
    using Network;
    using Network.Events;
    using Packets.Incoming;

    internal class PlayerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PlayerDao>();
            collection.AddSingleton<PlayerRepostiory>();
            collection.AddSingleton<IPlayerController, PlayerController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, UniqueIdEvent>();
            collection.AddSingleton<IAsyncPacket, SecureLoginEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserDataEvent>();
        }
    }
}
