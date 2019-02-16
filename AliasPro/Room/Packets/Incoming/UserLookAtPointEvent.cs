﻿using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Outgoing;
    using System;

    public class UserLookAtPointEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserLookAtPointMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int x = clientPacket.ReadInt();
            int y = clientPacket.ReadInt();

            if (x == session.Entity.Position.X &&
                y == session.Entity.Position.Y) return;

            if (session.Entity.ActiveStatuses.ContainsKey("mv") ||
                session.Entity.ActiveStatuses.ContainsKey("lay")) return;

            int newDir = session.Entity.Position.CalculateDirection(x, y);

            if (Math.Abs(newDir - session.Entity.BodyRotation) <= 2)
                session.Entity.HeadRotation = newDir;
            else
                session.Entity.BodyRotation = newDir;

            session.Entity.dirOffsetTimer = 0;
        }
    }
}