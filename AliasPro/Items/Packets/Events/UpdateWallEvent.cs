using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Items.Packets.Events
{
    public class UpdateWallEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateWallMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                string wallPosition = clientPacket.ReadString();
                item.ExtraData = wallPosition;
                await room.SendAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
