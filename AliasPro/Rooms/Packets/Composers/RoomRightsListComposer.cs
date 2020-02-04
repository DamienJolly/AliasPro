using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomRightsListComposer : IMessageComposer
    {
        private readonly int _roomId;
        private readonly IDictionary<uint, string> _rights;

        public RoomRightsListComposer(int roomId, IDictionary<uint, string> rights)
        {
            _roomId = roomId;
            _rights = rights;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomRightsListMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_rights.Count);
            foreach (var right in _rights)
            {
                message.WriteInt((int)right.Key);
                message.WriteString(right.Value);
            }
            return message;
        }
    }
}
