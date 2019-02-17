using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Outgoing;

    public class UserDanceEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserDanceMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int danceId = clientPacket.ReadInt();
            if (danceId < 0 || danceId > 5) return;

            room.EntityHandler.Unidle(session.Entity);
            session.Entity.DanceId = danceId;
            
            await room.SendAsync(new UserDanceComposer(session.Entity));
        }
    }
}
