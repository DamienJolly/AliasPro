using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAccessibleComposer : IMessageComposer
    {
        private readonly string _playerName;

        public RoomAccessibleComposer(string playerName)
        {
            _playerName = playerName;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomAccessibleMessageComposer);
            message.WriteString(_playerName);
            return message;
        }
    }
}
