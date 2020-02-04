using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomMutedComposer : IMessageComposer
    {
        private readonly bool _isMuted;

        public RoomMutedComposer(bool isMuted)
        {
            _isMuted = isMuted;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomMutedMessageComposer);
            message.WriteBoolean(_isMuted);
            return message;
        }
    }
}
