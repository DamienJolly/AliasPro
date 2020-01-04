using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomRightsListComposer : IPacketComposer
    {
        private readonly int _roomId;
        private readonly IDictionary<uint, string> _rights;

        public RoomRightsListComposer(int roomId, IDictionary<uint, string> rights)
        {
            _roomId = roomId;
            _rights = rights;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomRightsListMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_rights.Count);
            foreach (var right in _rights)
            {
                message.WriteInt(right.Key);
                message.WriteString(right.Value);
            }
            return message;
        }
    }
}
