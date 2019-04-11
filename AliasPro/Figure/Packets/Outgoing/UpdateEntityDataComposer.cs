namespace AliasPro.Figure.Packets.Outgoing
{
    using AliasPro.Player.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Room.Models.Entities;

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
