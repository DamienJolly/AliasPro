using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Packets.Composers
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
