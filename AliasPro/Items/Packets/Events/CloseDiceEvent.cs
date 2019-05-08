﻿using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Items.Packets.Events
{
    public class CloseDiceEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.CloseDiceMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (session.Entity == null) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.InteractionType != ItemInteractionType.DICE) return;

                item.Interaction.OnUserInteract(session.Entity, 0);
                room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);
            }
        }
    }
}