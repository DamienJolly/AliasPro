using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionCollision : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.COLLISION;
        
        public IWiredData WiredData { get; set; }

        public WiredInteractionCollision(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            IRoomPosition position = (IRoomPosition)args[0];

            if (_item.CurrentRoom.Mapping.TryGetRoomTile(position.X, position.Y, out IRoomTile roomTile))
            {
                foreach (BaseEntity entity in roomTile.Entities)
                {
                    _item.CurrentRoom.Items.TriggerEffects(roomTile, entity);
                }
            }
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
