﻿using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserSitEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserSitMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            if (session.Entity.Actions.HasStatus("mv") ||
                session.Entity.Actions.HasStatus("lay") ||
                session.Entity.Actions.HasStatus("sit")) return;

            if ((session.Entity.BodyRotation % 2) != 0)
                session.Entity.SetRotation(session.Entity.BodyRotation - 1);

            session.Entity.Actions.AddStatus("sit", 0.5 + "");
            session.Entity.IsSitting = true;
            session.Entity.NeedsUpdate = true;
        }
    }
}
