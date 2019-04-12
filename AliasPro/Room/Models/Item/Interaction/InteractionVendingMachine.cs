using System.Collections.Generic;

namespace AliasPro.Room.Models.Item.Interaction
{
    using Network.Protocol;
    using AliasPro.Items.Models;
    using Utilities;
    using AliasPro.Room.Models.Entities;
    using AliasPro.Items.Packets.Composers;
    using AliasPro.Room.Packets.Composers;

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

            if (!_item.CurrentRoom.RoomMap.TilesAdjecent(_item.Position, entity.Position))
            {
                //todo: walk to item
                return;
            }

            if (_item.Mode == 1) return;

            if (!entity.Actions.HasStatus("sit") &&
                !entity.Actions.HasStatus("lay"))
            {
                entity.Actions.RemoveStatus("mv");
                entity.Position.CalculateDirection(_item.Position);
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
            foreach(string item in extraData.Split(','))
                items.Add(int.Parse(item));

            if (items.Count != 0)
                id = items[Randomness.RandomNumber(items.Count) - 1];
            return id;
        }
    }
}
