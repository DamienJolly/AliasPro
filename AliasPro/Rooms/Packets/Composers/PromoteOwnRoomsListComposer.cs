using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class PromoteOwnRoomsListComposer : IPacketComposer
    {
        private readonly ICollection<IRoomData> _rooms;

        public PromoteOwnRoomsListComposer(ICollection<IRoomData> rooms)
        {
            _rooms = rooms;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PromoteOwnRoomsListMessageComposer);
            message.WriteBoolean(false); //??
            message.WriteInt(_rooms.Count);
            foreach (IRoomData room in _rooms)
            {
                message.WriteInt(room.Id);
                message.WriteString(room.Name);
                message.WriteBoolean(room.IsPromoted);
            }
            return message;
        }
    }
}
