﻿using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Composers
{
    public class RoomInviteComposer : IPacketComposer
    {
        private readonly uint _playerId;
        private readonly string _message;

        public RoomInviteComposer(uint playerId, string message)
        {
            _playerId = playerId;
            _message = message;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomInviteMessageComposer);
            message.WriteInt(_playerId);
            message.WriteString(_message);
            return message;
        }
    }
}