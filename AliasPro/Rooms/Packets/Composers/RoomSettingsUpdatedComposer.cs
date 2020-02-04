using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomSettingsUpdatedComposer : IMessageComposer
    {
        private readonly uint _roomId;

        public RoomSettingsUpdatedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomSettingsUpdatedMessageComposer);
            message.WriteInt((int)_roomId);
            return message;
        }
    }
}
