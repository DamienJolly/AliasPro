﻿using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestBotInventoryEvent : IMessageEvent
    {
        public short Header => Incoming.RequestBotInventoryMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            
            await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
        }
    }
}
