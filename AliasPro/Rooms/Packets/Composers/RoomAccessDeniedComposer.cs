using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAccessDeniedComposer : IMessageComposer
    {
        private readonly string _playerName;

        public RoomAccessDeniedComposer(string playerName)
        {
            _playerName = playerName;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomAccessDeniedMessageComposer);
            message.WriteString(_playerName);
            return message;
        }
    }
}
