using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAccessibleComposer : IPacketComposer
    {
        private readonly string _playerName;

        public RoomAccessibleComposer(string playerName)
        {
            _playerName = playerName;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomAccessibleMessageComposer);
            message.WriteString(_playerName);
            return message;
        }
    }
}
