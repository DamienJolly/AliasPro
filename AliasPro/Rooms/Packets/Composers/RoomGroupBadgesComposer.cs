using AliasPro.API.Groups.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomGroupBadgesComposer : IMessageComposer
    {
        private readonly ICollection<IGroup> _groups;

        public RoomGroupBadgesComposer(ICollection<IGroup> groups)
        {
            _groups = groups;
        }

        public RoomGroupBadgesComposer(IGroup group)
        {
            _groups = new List<IGroup>();
            _groups.Add(group);
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomGroupBadgesMessageComposer);
            message.WriteInt(_groups.Count);
            foreach (IGroup group in _groups)
            {
                message.WriteInt(group.Id);
                message.WriteString(group.Badge);
            }
            return message;
        }
    }
}
