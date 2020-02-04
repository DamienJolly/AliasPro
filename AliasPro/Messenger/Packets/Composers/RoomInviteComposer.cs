using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Messenger.Packets.Composers
{
    public class RoomInviteComposer : IMessageComposer
    {
        private readonly uint _playerId;
        private readonly string _message;

        public RoomInviteComposer(uint playerId, string message)
        {
            _playerId = playerId;
            _message = message;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomInviteMessageComposer);
            message.WriteInt((int)_playerId);
            message.WriteString(_message);
            return message;
        }
    }
}
