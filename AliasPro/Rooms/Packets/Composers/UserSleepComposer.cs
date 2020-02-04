using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserSleepComposer : IMessageComposer
    {
        private readonly BaseEntity _entity;

        public UserSleepComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserSleepMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteBoolean(_entity.IsIdle);
            return message;
        }
    }
}
