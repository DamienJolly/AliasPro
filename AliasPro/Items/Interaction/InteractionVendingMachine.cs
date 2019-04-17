using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionVendingMachine : IItemInteractor
    {
        private readonly IItem _item;

        private int _tickCount = 0;

        public InteractionVendingMachine(IItem item)
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

            if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
                return;

            IRoomPosition position = tile.PositionInFront(_item.Rotation);
            if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
            {
                entity.GoalPosition = position;
                return;
            }

            if (_item.Mode == 1) return;

            if (!entity.Actions.HasStatus("sit") &&
                !entity.Actions.HasStatus("lay"))
            {
                entity.Actions.RemoveStatus("mv");
                entity.SetRotation(entity.Position.CalculateDirection(_item.Position.X, _item.Position.Y));
            }

            _item.Mode = 1;
            _item.InteractingPlayer = entity;
            _tickCount = 0;

            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

        public async void OnCycle()
        {
            if (_item.Mode != 1) return;

            if (_tickCount < 1)
            {
                _tickCount++;
                return;
            }

            _item.Mode = 0;
            int handItemId =
                GetRandomVendingMachineId(_item.ExtraData);
            _item.InteractingPlayer.SetHandItem(handItemId);

            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
            await _item.CurrentRoom.SendAsync(new UserHandItemComposer(
                _item.InteractingPlayer.Id,
                _item.InteractingPlayer.HandItemId));
        }

        private int GetRandomVendingMachineId(string extraData)
        {
            int id = 0;
            IList<int> items = new List<int>();
            foreach (string item in extraData.Split(','))
            {
                if (int.TryParse(item, out int itemId))
                    items.Add(itemId);
            }

            if (items.Count != 0)
                id = items[Randomness.RandomNumber(items.Count) - 1];
            return id;
        }
    }
}
