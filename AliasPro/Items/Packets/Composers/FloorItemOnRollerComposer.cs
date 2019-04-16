using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class FloorItemOnRollerComposer : IPacketComposer
    {
        private readonly IItem _item;
        private readonly IRoomPosition _target;
        private readonly uint _rollerId;

        public FloorItemOnRollerComposer(IItem item, IRoomPosition target, uint rollerId)
        {
            _item = item;
            _target = target;
            _rollerId = rollerId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.SlideObjectMessageComposer);
            message.WriteInt(_item.Position.X);
            message.WriteInt(_item.Position.Y);
            message.WriteInt(_target.X);
            message.WriteInt(_target.Y);
            message.WriteInt(1);
            message.WriteInt(_item.Id);
            message.WriteDouble(_item.Position.Z);
            message.WriteDouble(_target.Z);
            message.WriteInt(_rollerId);
            return message;
        }
    }
}
