using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class RoomSettingsUpdatedComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public RoomSettingsUpdatedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomSettingsUpdatedMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
