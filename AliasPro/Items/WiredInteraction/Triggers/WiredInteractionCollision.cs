using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

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
            Position position = (Position)args[0];

            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(position.X, position.Y, out RoomTile roomTile))
            {
                foreach (BaseEntity entity in roomTile.Entities)
                {
                    _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile, entity);
                }
            }
            return true;
        }

        public void OnCycle()
        {

        }
    }
}
