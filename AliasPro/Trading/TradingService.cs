using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Trading;
using AliasPro.Trading.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Trading
{
    internal class TradingService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<TradingDao>();
            collection.AddSingleton<ITradingController, TradingController>();

			AddPackets(collection);
		}

		private static void AddPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IAsyncPacket, TradeStartEvent>();
			collection.AddSingleton<IAsyncPacket, TradeOfferItemEvent>();
			collection.AddSingleton<IAsyncPacket, TradeOfferMultipleItemsEvent>();
			collection.AddSingleton<IAsyncPacket, TradeCancelOfferItemEvent>();
			collection.AddSingleton<IAsyncPacket, TradeAcceptEvent>();
			collection.AddSingleton<IAsyncPacket, TradeUnAcceptEvent>();

			collection.AddSingleton<IAsyncPacket, TradeCloseEvent>();
		}
	}
}
