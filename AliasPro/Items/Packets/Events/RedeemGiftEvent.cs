using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Tasks;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
	public class RedeemGiftEvent : IMessageEvent
	{
        public short Header => Incoming.RedeemGiftMessageEvent;

		private readonly IItemController _itemController;

		public RedeemGiftEvent(
			IItemController itemController)
		{
			_itemController = itemController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
				return;

			if (!session.CurrentRoom.Rights.IsOwner(session.Player.Id)) 
				return;

			uint itemId = (uint)message.ReadInt();
			if (!room.Items.TryGetItem(itemId, out IItem item)) 
				return;

			if (item.ItemData.Type != "s") 
				return;

			if (item.Interaction is InteractionGift giftInteraction)
			{
				if (item.ItemData.Name.Contains("present_wrap"))
				{
					giftInteraction.Exploaded = true;
					await room.SendPacketAsync(new FloorItemUpdateComposer(item));
				}

				if (!_itemController.TryGetItemDataById((uint)giftInteraction.itemId, out IItemData giftData))
					return;

				await TaskManager.ExecuteTask(new OpenGiftTask(giftData, giftInteraction.ExtraData, item, session), giftInteraction.Exploaded ? 1500 : 0);
			}
			else if (item.Interaction is InteractionEcotron ecotronInteraction)
			{
				if (!_itemController.TryGetItemDataById((uint)ecotronInteraction.itemId, out IItemData giftData))
					return;

				string extradata = ecotronInteraction.ExtraData;

				room.RoomGrid.RemoveItem(item);
				room.Items.RemoveItem(item.Id);
				await room.SendPacketAsync(new RemoveFloorItemComposer(item));

				item.ItemData = giftData;
				item.ItemId = giftData.Id;
				item.ExtraData = "";
				item.Mode = 0;
				item.Interaction = null;

				if (session.Player.Inventory.TryAddItem(item))
				{
					await session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)item.Id));
					await session.SendPacketAsync(new InventoryRefreshComposer());
				}

				await session.SendPacketAsync(new PresentItemOpenedComposer(item, extradata, false));
			}
		}
    }
}
