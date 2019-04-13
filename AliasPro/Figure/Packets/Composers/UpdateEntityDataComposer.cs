using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Composers
{
    public class UpdateEntityDataComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;

        public UpdateEntityDataComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateEntityDataMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteString(_entity.Figure);
            message.WriteString(_entity.Gender == PlayerGender.MALE ? "m" : "f");
            message.WriteString(_entity.Motto);
            message.WriteInt(0); //todo: ach. score
            return message;
        }
    }
}
