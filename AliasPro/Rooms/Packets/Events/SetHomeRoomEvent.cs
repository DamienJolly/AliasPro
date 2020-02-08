using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class SetHomeRoomEvent : IMessageEvent
    {
        public short Header => Incoming.SetHomeRoomMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int roomId = message.ReadInt();

            if (roomId == session.Player.HomeRoom)
                return;

            session.Player.HomeRoom = roomId;
            await session.SendPacketAsync(new HomeRoomComposer(session.Player.HomeRoom));
        }
    }
}
