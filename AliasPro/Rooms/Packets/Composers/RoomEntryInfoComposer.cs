using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomEntryInfoComposer : IMessageComposer
    {
        private readonly uint _roomId;
        private readonly bool _hasRights;

        public RoomEntryInfoComposer(uint roomId, bool hasRights)
        {
            _roomId = roomId;
            _hasRights = hasRights;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomEntryInfoMessageComposer);
            message.WriteInt((int)_roomId);
            message.WriteBoolean(_hasRights);
            return message;
        }
    }
}
