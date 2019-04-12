using AliasPro.Items.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionWalksOn : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.WALKS_ON_FURNI;

        public WiredData WiredData { get; set; }

        public WiredInteractionWalksOn(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            IItem item = (IItem)args[1];
            if (item == null) return false;

            if (!WiredData.Items.ContainsKey(item.Id)) return false;

            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
            {
                _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile, entity);
            }
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
