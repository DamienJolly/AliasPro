using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomRightsComposer : IMessageComposer
    {
        private readonly int _rightLevel;

        public RoomRightsComposer(int rightLevel)
        {
            _rightLevel = rightLevel;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomRightsMessageComposer);
            message.WriteInt(_rightLevel);
            return message;
        }
    }
}
