using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
using System.Collections.Generic;

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
            
            IList<Position> walkingPath = Pathfinder.FindPath(
                session.CurrentRoom.RoomGrid,
                new Position(
                    session.Entity.Position.X, 
                    session.Entity.Position.Y),
                new Position(x, y),
                DiagonalMovement.ONE_WALKABLE,
                session.Entity);

            session.Entity.PathToWalk = walkingPath;
        }
    }
}
