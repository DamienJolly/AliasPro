﻿using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class UniqueIdEvent : IMessageEvent
    {
        public short Header => Incoming.UniqueIdMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            message.ReadString();
            session.UniqueId = message.ReadString();
            return Task.CompletedTask;
        }
    }
}
