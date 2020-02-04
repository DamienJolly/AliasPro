using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomChatSettingsComposer : IMessageComposer
    {
        private readonly IRoomSettings _roomSettings;

        public RoomChatSettingsComposer(IRoomSettings roomSettings)
        {
            _roomSettings = roomSettings;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomChatSettingsMessageComposer);
            message.WriteInt(_roomSettings.ChatMode);
            message.WriteInt(_roomSettings.ChatSize);
            message.WriteInt(_roomSettings.ChatSpeed);
            message.WriteInt(_roomSettings.ChatDistance);
            message.WriteInt(_roomSettings.ChatFlood);
            return message;
        }
    }
}
