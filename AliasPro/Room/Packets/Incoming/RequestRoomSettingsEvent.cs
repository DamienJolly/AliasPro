using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Outgoing;
    using Models;
    using Sessions;

    public class RequestRoomSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomSettingsMessageEvent;

        public async Task HandleAsync(
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
