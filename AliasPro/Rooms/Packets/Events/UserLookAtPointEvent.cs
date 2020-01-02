﻿using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using System;

namespace AliasPro.Rooms.Packets.Events
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


            int newDir = session.Entity.BodyRotation == 0 ? 8 : session.Entity.Position.CalculateDirection(x, y);
            bool headonly = Math.Abs(newDir - session.Entity.BodyRotation) <= 1;

            session.Entity.SetRotation(newDir, headonly);
            session.Entity.Unidle();
            session.Entity.DirOffsetTimer = 0;
        }
    }
}
