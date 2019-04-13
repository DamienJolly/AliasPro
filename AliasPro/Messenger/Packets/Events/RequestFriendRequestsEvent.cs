﻿using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestFriendRequestsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFriendRequestsMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new LoadFriendRequestsComposer(session.Player.Messenger.Requests));
        }
    }
}
