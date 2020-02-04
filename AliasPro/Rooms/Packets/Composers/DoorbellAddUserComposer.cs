using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class DoorbellAddUserComposer : IMessageComposer
    {
        private readonly string _playerName;

        public DoorbellAddUserComposer(string playerName)
        {
            _playerName = playerName;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.DoorbellAddUserMessageComposer);
            message.WriteString(_playerName);
            return message;
        }
    }
}
