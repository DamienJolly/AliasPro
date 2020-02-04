using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class FloorItemOnRollerComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.SlideObjectMessageComposer);
            message.WriteInt(_item.Position.X);
            message.WriteInt(_item.Position.Y);
            message.WriteInt(_target.X);
            message.WriteInt(_target.Y);
            message.WriteInt(1);
            message.WriteInt((int)_item.Id);
            message.WriteString(_item.Position.Z.ToString());
            message.WriteString(_target.Z.ToString());
            message.WriteInt((int)_rollerId);
            return message;
        }
    }
}
