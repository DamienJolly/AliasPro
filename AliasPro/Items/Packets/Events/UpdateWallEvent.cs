using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Sessions;

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
