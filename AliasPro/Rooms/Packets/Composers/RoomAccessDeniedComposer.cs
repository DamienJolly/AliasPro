using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAccessDeniedComposer : IPacketComposer
    {
        private readonly string _playerName;

        public RoomAccessDeniedComposer(string playerName)
        {
            _playerName = playerName;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomAccessDeniedMessageComposer);
            message.WriteString(_playerName);
            return message;
        }
    }
}
