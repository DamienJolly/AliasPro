﻿using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Utilities;

namespace AliasPro.Messenger.Packets.Composers
{
    public class FriendChatComposer : IPacketComposer
    {
        private readonly IMessengerMessage _privateMessage;

        public FriendChatComposer(IMessengerMessage privateMessage)
        {
            _privateMessage = privateMessage;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FriendChatMessageComposer);
            message.WriteInt(_privateMessage.TargetId);
            message.WriteString(_privateMessage.Message);
            message.WriteInt((int)UnixTimestamp.Now - 
                _privateMessage.Timestamp);
            return message;
        }
    }
}