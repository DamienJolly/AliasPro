using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Composers
{
    public class UserSearchResultComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerData> _friends;
        private readonly ICollection<IPlayerData> _notFriends;

        public UserSearchResultComposer(
			ICollection<IPlayerData> friends, 
			ICollection<IPlayerData> notFriends)
        {
            _friends = friends;
            _notFriends = notFriends;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserSearchResultMessageComposer);
            message.WriteInt(_friends.Count);
            foreach (IPlayerData player in _friends)
            {
                message.WriteInt(player.Id);
                message.WriteString(player.Username);
                message.WriteString(player.Motto);
                message.WriteBoolean(false); //online
                message.WriteBoolean(false);
                message.WriteString("");
                message.WriteInt(0);
                message.WriteString(player.Figure);
                message.WriteString("01.01.1970 00:00:00"); //LastOnline
            }
            message.WriteInt(_notFriends.Count);
            foreach (IPlayerData player in _notFriends)
            {
                message.WriteInt(player.Id);
                message.WriteString(player.Username);
                message.WriteString(player.Motto);
                message.WriteBoolean(false); //online
                message.WriteBoolean(false);
                message.WriteString("");
                message.WriteInt(0);
                message.WriteString(player.Figure);
                message.WriteString("01.01.1970 00:00:00"); //LastOnline
            }
            return message;
        }
    }
}
