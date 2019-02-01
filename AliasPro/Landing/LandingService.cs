using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Landing
{
    using Packets.Incoming;
    using Network.Events;
    using Network;

    internal class LandingService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<LandingDao>();
            collection.AddSingleton<LandingRepository>();
            collection.AddSingleton<ILandingController, LandingController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, RequestNewsListEvent>();
            collection.AddSingleton<IAsyncPacket, HotelViewEvent>();
        }
    }
}
