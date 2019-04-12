using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Packets.Composers
{
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
