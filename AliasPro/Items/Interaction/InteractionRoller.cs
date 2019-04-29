using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Models;
using System.Linq;

namespace AliasPro.Items.Interaction
{
    public class InteractionRoller : IItemInteractor
    {
        private readonly IItem _item;

        private int _tick = 2;

        public InteractionRoller(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }

        public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
            _item.Mode++;
            if (_item.Mode >= _item.ItemData.Modes)
            {
                _item.Mode = 0;
            }

            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

        public async void OnCycle()
        {
            _tick--;
            if (_tick <= 0)
            {
                if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile rollerTile))
                    return;
                
                foreach (IItem item in rollerTile.Items.ToList())
                {
                    if (item.Id == _item.Id) continue;

                    IRoomPosition newPos = HandleMovement(_item.Rotation, item.Position);
                    _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                    if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                        !_item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                        continue;

                    newPos.Z = roomTile.Height;
                    await _item.CurrentRoom.SendAsync(new FloorItemOnRollerComposer(item, newPos, _item.Id));

                    _item.CurrentRoom.RoomGrid.RemoveItem(item);
                    item.Position = newPos;
                    _item.CurrentRoom.RoomGrid.AddItem(item);
                }

                //todo: users

                _tick = 2;
            }
        }

        private IRoomPosition HandleMovement(int rotation, IRoomPosition position)
        {
            IRoomPosition newPos =
                new RoomPosition(position.X, position.Y, position.Z);

            switch (rotation)
            {
                case 0: newPos.Y--; break;
                case 2: newPos.X++; break;
                case 4: newPos.Y++; break;
                case 6: newPos.X--; break;
            }

            return newPos;
        }
    }
}
