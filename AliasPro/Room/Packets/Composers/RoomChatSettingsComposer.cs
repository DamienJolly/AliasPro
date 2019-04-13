using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Models;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomChatSettingsComposer : IPacketComposer
    {
        private readonly IRoomSettings _roomSettings;

        public RoomChatSettingsComposer(IRoomSettings roomSettings)
        {
            _roomSettings = roomSettings;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomChatSettingsMessageComposer);
            message.WriteInt(_roomSettings.ChatMode);
            message.WriteInt(_roomSettings.ChatSize);
            message.WriteInt(_roomSettings.ChatSpeed);
            message.WriteInt(_roomSettings.ChatDistance);
            message.WriteInt(_roomSettings.ChatFlood);
            return message;
        }
    }
}
