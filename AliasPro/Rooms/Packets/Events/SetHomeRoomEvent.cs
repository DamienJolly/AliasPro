using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class SetHomeRoomEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SetHomeRoomMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();

            if (roomId == session.Player.HomeRoom)
                return;

            session.Player.HomeRoom = roomId;
            await session.SendPacketAsync(new HomeRoomComposer(session.Player.HomeRoom));
        }
    }
}
