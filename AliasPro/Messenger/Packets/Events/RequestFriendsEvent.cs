﻿using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestFriendsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestFriendsMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            //not used??
            return Task.CompletedTask;
        }
    }
}
