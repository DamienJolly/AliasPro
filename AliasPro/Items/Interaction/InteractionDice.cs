using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction
{
    public class InteractionDice : IItemInteractor
    {
        private readonly IItem _item;

        private int _tickCount = 0;

        public InteractionDice(IItem item)
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
            if (entity == null) return;

            if (_item.Mode == -1) return;

            if (_item.Mode == 0 && state == 0) return;

            if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
                return;

            IRoomPosition position = tile.PositionInFront(_item.Rotation);
            if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
            {
                entity.GoalPosition = position;
                return;
            }

            _item.Mode = state;
            _tickCount = 0;
            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

        public async void OnCycle()
        {
            if (_item.Mode == -1)
            {
                if (_tickCount >= 2)
                {
                    _item.Mode = Randomness.RandomNumber(_item.ItemData.Modes);
                    await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
                }
                _tickCount++;
            }
        }
    }
}
