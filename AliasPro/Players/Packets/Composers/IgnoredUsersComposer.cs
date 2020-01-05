using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class IgnoredUsersComposer : IPacketComposer
    {
        private readonly ICollection<string> _ignoredUsers;

        public IgnoredUsersComposer(ICollection<string> ignoredUsers)
        {
            _ignoredUsers = ignoredUsers;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.IgnoredUsersMessageComposer);
            message.WriteInt(_ignoredUsers.Count);
            foreach (string username in _ignoredUsers)
            {
                message.WriteString(username);
            }
			return message;
        }
    }
}
