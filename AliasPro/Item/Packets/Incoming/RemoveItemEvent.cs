using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;

    public class RemoveItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RemoveItemMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadInt(); //??
            uint itemId = (uint)clientPacket.ReadInt();

            IRoom room = session.CurrentRoom;
            if (room.RoomItems.TryGetValue(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    await room.RemoveItem(item);
                }
                else
                {
                    //todo: wall items
                }

                await session.Player.Inventory.AddItem(item);
                await session.SendPacketAsync(new InventoryRefreshComposer());
            }
        }
    }
}
