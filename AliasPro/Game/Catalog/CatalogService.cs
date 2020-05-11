using AliasPro.Communication.Messages;
using AliasPro.Game.Catalog.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Catalog
{
	public class CatalogService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<CatalogDao>();
            collection.AddSingleton<CatalogController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestDiscountEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogIndexEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogModeEvent>();
            collection.AddSingleton<IMessageEvent, RequestCatalogPageEvent>();
            collection.AddSingleton<IMessageEvent, CatalogBuyItemEvent>();
            //collection.AddSingleton<IMessageEvent, CatalogBuyGiftItemEvent>();
            collection.AddSingleton<IMessageEvent, RequestGiftConfigurationEvent>();
            //collection.AddSingleton<IMessageEvent, RequestRecyclerLogicEvent>();
            //collection.AddSingleton<IMessageEvent, ReloadRecyclerEvent>();
            //collection.AddSingleton<IMessageEvent, RecycleEvent>();
            collection.AddSingleton<IMessageEvent, CatalogSearchedItemEvent>();
        }
    }
}
