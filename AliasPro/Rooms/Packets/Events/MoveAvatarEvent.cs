using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
{
    public class MoveAvatarEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.MoveAvatarMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int x = clientPacket.ReadInt();
            int y = clientPacket.ReadInt();

            if (session.Entity.Position.X == x &&
                session.Entity.Position.Y == y) return;

            session.Entity.FindPath(x, y);
        }
    }
}
