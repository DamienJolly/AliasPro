using AliasPro.API.Landing;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Landing.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Landing
{
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
