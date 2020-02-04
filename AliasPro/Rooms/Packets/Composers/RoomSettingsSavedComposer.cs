using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomSettingsSavedComposer : IMessageComposer
    {
        private readonly uint _roomId;

        public RoomSettingsSavedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomSettingsSavedMessageComposer);
            message.WriteInt((int)_roomId);
            return message;
        }
    }
}
