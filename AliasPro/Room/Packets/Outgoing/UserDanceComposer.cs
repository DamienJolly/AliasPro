namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

    public class UserDanceComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;

        public UserDanceComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserDanceMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteInt(_entity.DanceId);
            return message;
        }
    }
}
