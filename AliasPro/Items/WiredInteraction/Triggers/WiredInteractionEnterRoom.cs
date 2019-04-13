using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionEnterRoom : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.ENTER_ROOM;
        
        public IWiredData WiredData { get; set; }

        public WiredInteractionEnterRoom(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (args.Length != 1) return false;

            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

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
