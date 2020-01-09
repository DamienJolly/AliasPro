using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomGroupBadgesComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomGroupBadgesMessageComposer);
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
