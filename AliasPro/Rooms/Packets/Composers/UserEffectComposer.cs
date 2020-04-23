using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserEffectComposer : IMessageComposer
    {
        private readonly int _virtualId;
        private readonly int _effectId;

        public UserEffectComposer(int virtualId, int effectId)
        {
            _virtualId = virtualId;
            _effectId = effectId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserEffectMessageComposer);
            message.WriteInt(_virtualId);
            message.WriteInt(_effectId);
            message.WriteInt(0);
            return message;
        }
    }
}
