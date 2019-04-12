using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Gamemap.Pathfinding;
using AliasPro.Room.Models;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Room.Packets.Events
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
            
            session.Entity.Position = session.Entity.NextPosition;
            IList<Position> walkingPath = PathFinder.FindPath(
                session.Entity,
                session.CurrentRoom.RoomMap,
                session.Entity.Position, new Position(x, y, 0));
            
            session.Entity.PathToWalk = walkingPath;
        }
    }
}
