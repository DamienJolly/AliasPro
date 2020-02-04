using AliasPro.API.Landing;
using AliasPro.Communication.Messages;
using AliasPro.Landing.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Landing
{
    internal class LandingService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<LandingDao>();
            collection.AddSingleton<LandingRepository>();
            collection.AddSingleton<ILandingController, LandingController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestNewsListEvent>();
            collection.AddSingleton<IMessageEvent, HotelViewEvent>();
			collection.AddSingleton<IMessageEvent, HotelViewDataEvent>();
		}
    }
}
