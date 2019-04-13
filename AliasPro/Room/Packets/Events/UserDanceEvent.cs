using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;

namespace AliasPro.Room.Packets.Events
{
    public class UserDanceEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserDanceMessageEvent;

        public async void HandleAsync(
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
