using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class RemoveItemEvent : IMessageEvent
    {
        public short Header => Incoming.RemoveItemMessageEvent;

        private readonly IItemController _itemController;
        private readonly IPlayerController _playerController;
        public RemoveItemEvent(
            IItemController itemController,
            IPlayerController playerController)
        {
            _itemController = itemController;
            _playerController = playerController;
        }
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            message.ReadInt(); //??
            uint itemId = (uint)message.ReadInt();

            IRoom room = session.CurrentRoom;

            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.PlayerId != session.Player.Id && !room.Rights.IsOwner(session.Player.Id))
                    return;

                if (item.ItemData.Type == "s")
                {
                    room.RoomGrid.RemoveItem(item);
                    await room.SendPacketAsync(new RemoveFloorItemComposer(item));
                }
                else
                {
                    await room.SendPacketAsync(new RemoveWallItemComposer(item));
                }

                item.Interaction.OnPickupItem();

                item.RoomId = 0;
                item.CurrentRoom = null;
                room.Items.RemoveItem(item.Id);
                await _itemController.UpdatePlayerItemAsync(item);

                if (_playerController.TryGetPlayer(item.PlayerId, out IPlayer targetPlayer))
                {
                    if (targetPlayer.Inventory.TryAddItem(item))
                    {
                        await targetPlayer.Session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)item.Id));
                        await targetPlayer.Session.SendPacketAsync(new InventoryRefreshComposer());
                    }
                }
            }
        }
    }
}
