using AliasPro.API.Trading;
using AliasPro.Communication.Messages;
using AliasPro.Trading.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Trading
{
    internal class TradingService : IService
	{
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<TradingDao>();
            collection.AddSingleton<ITradingController, TradingController>();

			RegisterPackets(collection);
		}

		private static void RegisterPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IMessageEvent, TradeStartEvent>();
			collection.AddSingleton<IMessageEvent, TradeOfferItemEvent>();
			collection.AddSingleton<IMessageEvent, TradeOfferMultipleItemsEvent>();
			collection.AddSingleton<IMessageEvent, TradeCancelOfferItemEvent>();
			collection.AddSingleton<IMessageEvent, TradeAcceptEvent>();
			collection.AddSingleton<IMessageEvent, TradeUnAcceptEvent>();
			collection.AddSingleton<IMessageEvent, TradeConfirmEvent>();
			collection.AddSingleton<IMessageEvent, TradeCloseEvent>();
		}
	}
}
