using AliasPro.API.Items;
using AliasPro.Communication.Messages;
using AliasPro.Items.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Items
{
    internal class ItemService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ItemDao>();
            collection.AddSingleton<IItemController, ItemController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, PlaceItemEvent>();
            collection.AddSingleton<IMessageEvent, UpdateItemEvent>();
            collection.AddSingleton<IMessageEvent, RemoveItemEvent>();
            collection.AddSingleton<IMessageEvent, UpdateWallEvent>();
            collection.AddSingleton<IMessageEvent, ToggleFloorItemEvent>();
            collection.AddSingleton<IMessageEvent, ToggleWallItemEvent>();
            collection.AddSingleton<IMessageEvent, ToggleDiceEvent>();
            collection.AddSingleton<IMessageEvent, ToggleOneWayEvent>();
            collection.AddSingleton<IMessageEvent, CloseDiceEvent>();
			collection.AddSingleton<IMessageEvent, RedeemItemEvent>();
			collection.AddSingleton<IMessageEvent, LoveLockConfirmEvent>();
			collection.AddSingleton<IMessageEvent, SetStackToolHeightEvent>();
			collection.AddSingleton<IMessageEvent, RedeemClothingEvent>();
			collection.AddSingleton<IMessageEvent, RedeemGiftEvent>();
			collection.AddSingleton<IMessageEvent, ApplyDecorationEvent>();
			collection.AddSingleton<IMessageEvent, SetTonerEvent>();
		}
    }
}