using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
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
