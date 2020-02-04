using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserDanceComposer : IMessageComposer
    {
        private readonly BaseEntity _entity;

        public UserDanceComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserDanceMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteInt(_entity.DanceId);
            return message;
        }
    }
}
