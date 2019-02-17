namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

    public class UserActionComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;
        private readonly int _action;

        public UserActionComposer(BaseEntity entity, int action)
        {
            _entity = entity;
            _action = action;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserActionMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteInt(_action);
            return message;
        }
    }
}
