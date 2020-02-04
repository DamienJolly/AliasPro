using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserActionComposer : IMessageComposer
    {
        private readonly BaseEntity _entity;
        private readonly int _action;

        public UserActionComposer(BaseEntity entity, int action)
        {
            _entity = entity;
            _action = action;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserActionMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteInt(_action);
            return message;
        }
    }
}
