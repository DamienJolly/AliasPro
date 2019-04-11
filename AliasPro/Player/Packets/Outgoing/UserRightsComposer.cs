﻿namespace AliasPro.Player.Packets.Outgoing
{
    using AliasPro.API.Player.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserRightsComposer : IPacketComposer
    {
        private readonly IPlayer _player;

        public UserRightsComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserRightsMessageComposer);
            message.WriteInt(2); //todo: subscription
            message.WriteInt(_player.Rank);
            message.WriteBoolean(false); //todo: ambassador
            return message;
        }
    }
}
