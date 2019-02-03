using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Room.Packets.Incoming
{
    using Gamemap;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Gamemap.Pathfinding;

    public class MoveAvatarEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.MoveAvatarMessageEvent;
        
        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int x = clientPacket.ReadInt();
            int y = clientPacket.ReadInt();
            session.Entity.Position = session.Entity.NextPosition;
            IList<Position> walkingPath = PathFinder.FindPath(
                session.CurrentRoom.RoomMap,
                session.Entity.Position, new Position(x, y, 0));
            session.Entity.PathToWalk = walkingPath;
            return Task.CompletedTask;
        }
    }
}
