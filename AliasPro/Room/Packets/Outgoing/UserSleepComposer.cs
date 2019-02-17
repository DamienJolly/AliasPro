namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

    public class UserSleepComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;

        public UserSleepComposer(BaseEntity entity)
        {
            _entity = entity;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserSleepMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteBoolean(_entity.IsIdle);
            return message;
        }
    }
}
