using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using System;

namespace AliasPro.Room.Packets.Events
{
    public class UserLookAtPointEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserLookAtPointMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int x = clientPacket.ReadInt();
            int y = clientPacket.ReadInt();

            if (x == session.Entity.Position.X &&
                y == session.Entity.Position.Y) return;

            if (session.Entity.Actions.HasStatus("mv") ||
                session.Entity.Actions.HasStatus("lay")) return;

            int newDir = session.Entity.Position.CalculateDirection(x, y);

            if (Math.Abs(newDir - session.Entity.BodyRotation) <= 2)
            {
                session.Entity.HeadRotation = newDir;
            }
            else
            {
                if (session.Entity.Actions.HasStatus("sit"))
                {
                    session.Entity.BodyRotation = newDir;
                    session.Entity.HeadRotation = newDir;
                }
            }

            room.EntityHandler.Unidle(session.Entity);
            session.Entity.DirOffsetTimer = 0;
        }
    }
}
