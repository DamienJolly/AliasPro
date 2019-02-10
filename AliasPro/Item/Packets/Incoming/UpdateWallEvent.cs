using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Outgoing;

    public class UpdateWallEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateWallMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                string wallPosition = clientPacket.ReadString();
                item.ExtraData = wallPosition;
                await room.SendAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
