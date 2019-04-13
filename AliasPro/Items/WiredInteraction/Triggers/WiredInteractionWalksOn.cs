using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.WiredInteraction
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

        public bool OnTrigger(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            IItem item = (IItem)args[1];
            if (item == null) return false;

            if (!WiredData.Items.ContainsKey(item.Id)) return false;

            if (_item.CurrentRoom.Mapping.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile roomTile))
            {
                _item.CurrentRoom.Items.TriggerEffects(roomTile, entity);
            }
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
