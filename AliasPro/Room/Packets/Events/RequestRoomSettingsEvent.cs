using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class RequestRoomSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomSettingsMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            if (!room.RightHandler.IsOwner(session.Player.Id)) return;

            await session.SendPacketAsync(new RoomSettingsComposer(room.RoomData));
        }
    }
}
