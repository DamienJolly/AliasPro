using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class IgnoredUsersComposer : IMessageComposer
    {
        private readonly ICollection<string> _ignoredUsers;

        public IgnoredUsersComposer(ICollection<string> ignoredUsers)
        {
            _ignoredUsers = ignoredUsers;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.IgnoredUsersMessageComposer);
            message.WriteInt(_ignoredUsers.Count);
            foreach (string username in _ignoredUsers)
            {
                message.WriteString(username);
            }
			return message;
        }
    }
}
