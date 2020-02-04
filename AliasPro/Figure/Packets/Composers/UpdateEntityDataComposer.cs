using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Composers
{
    public class UpdateEntityDataComposer : IMessageComposer
    {
        private readonly BaseEntity _entity;

        public UpdateEntityDataComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UpdateEntityDataMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteString(_entity.Figure);
            message.WriteString(_entity.Gender == PlayerGender.MALE ? "m" : "f");
            message.WriteString(_entity.Motto);
            message.WriteInt(_entity.Score);
            return message;
        }
    }
}
