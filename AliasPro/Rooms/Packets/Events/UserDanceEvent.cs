using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserDanceEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UserDanceMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int danceId = clientPacket.ReadInt();
            if (danceId < 0 || danceId > 5) return;

            session.Entity.Unidle();
            session.Entity.DanceId = danceId;
            
            await room.SendPacketAsync(new UserDanceComposer(session.Entity));
        }
    }
}
