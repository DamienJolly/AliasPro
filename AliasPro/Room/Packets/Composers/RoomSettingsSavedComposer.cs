using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class RoomSettingsSavedComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public RoomSettingsSavedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomSettingsSavedMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
