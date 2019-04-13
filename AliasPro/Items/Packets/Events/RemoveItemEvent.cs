using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;

namespace AliasPro.Items.Packets.Events
{
    public class RemoveItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveItemMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadInt(); //??
            uint itemId = (uint)clientPacket.ReadInt();

            IRoom room = session.CurrentRoom;

            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    room.RoomMap.RemoveItem(item);
                    await room.SendAsync(new RemoveFloorItemComposer(item));
                }
                else
                {
                    await room.SendAsync(new RemoveWallItemComposer(item));
                }

                item.RoomId = 0;
                item.CurrentRoom = null;

                if(session.Player.Inventory.TryAddItem(item))
                {
                    room.ItemHandler.RemoveItem(item.Id);

                    await session.SendPacketAsync(new AddPlayerItemsComposer(item));
                    await session.SendPacketAsync(new InventoryRefreshComposer());
                }
            }
        }
    }
}
