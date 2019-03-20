using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionWalksOn : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.WALKS_ON_FURNI;

        public IWiredData WiredData { get; set; }

        public WiredInteractionWalksOn(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return;

            IItem item = (IItem)args[1];
            if (item == null) return;

            if (!WiredData.Items.Contains(item.Id)) return;

            foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(entity);
            }
        }

        public void OnCycle()
        {

        }
    }
}
