using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Room.Gamemap;
    using Sessions;
    using Outgoing;

    public class RemoveItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveItemMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            System.Console.WriteLine(session.Player.Inventory.Items.Count);
            clientPacket.ReadInt(); //??
            uint itemId = (uint)clientPacket.ReadInt();

            IRoom room = session.CurrentRoom;
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    if (!room.RoomMap.TryGetRoomTile(item.Position.X, item.Position.Y, out RoomTile tile)) return;

                    tile.RemoveItem(item.Id);
                    await room.SendAsync(new RemoveFloorItemComposer(item));
                }
                else
                {
                    await room.SendAsync(new RemoveWallItemComposer(item));
                }

                item.RoomId = 0;

                await session.Player.Inventory.AddItem(item);

                room.ItemHandler.RemoveItem(item.Id);

                await session.SendPacketAsync(new AddPlayerItemsComposer(item));
                await session.SendPacketAsync(new InventoryRefreshComposer());
            }
        }
    }
}
