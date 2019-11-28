using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Items.Packets.Events
{
    public class RedeemClothingEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RedeemClothingMessageEvent;

		private readonly IFigureController _figureController;
		private readonly IItemController _itemController;

		public RedeemClothingEvent(
			IFigureController figureController,
			IItemController itemController)
		{
			_figureController = figureController;
			_itemController = itemController;
		}

		public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (session.Entity == null) return;

			if (!session.CurrentRoom.Rights.IsOwner(session.Player.Id)) return;

			uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type != "s") return;

                if (item.ItemData.InteractionType != ItemInteractionType.CLOTHING) return;

				if (string.IsNullOrEmpty(item.ItemData.ExtraData)) return;

				if (!int.TryParse(item.ItemData.ExtraData, out int clothingId)) return;

				if (!_figureController.TryGetClothingItem(clothingId, out IClothingItem clothingItem)) return;

				if (session.Player.Wardrobe.TryGetClothingItem(clothingItem.Id, out _))
				{
					await session.SendPacketAsync(new BubbleAlertComposer(BubbleAlertComposer.FIGURESET_OWNED_ALREADY));
					return;
				}

				room.RoomGrid.RemoveItem(item);
				room.Items.RemoveItem(item.Id);
				await room.SendAsync(new RemoveFloorItemComposer(item));
				await _itemController.RemoveItemAsync(item);

				if (session.Player.Wardrobe.TryAddClothingItem(clothingItem))
					await _figureController.AddClothingItemAsync(session.Player.Id, clothingItem);

				await session.SendPacketAsync(new UserClothesComposer(session.Player.Wardrobe.ClothingItems));
				await session.SendPacketAsync(new BubbleAlertComposer(BubbleAlertComposer.FIGURESET_REDEEMED));
			}
		}
    }
}
