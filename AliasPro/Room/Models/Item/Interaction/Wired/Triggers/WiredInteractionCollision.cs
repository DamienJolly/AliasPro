using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
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

        public void OnTrigger(params object[] args)
        {
            Position position = (Position)args[0];

            RoomTile roomTile = _item.CurrentRoom.RoomMap.GetRoomTile(
                    position.X,
                    position.Y);

            foreach (IItem effect in _item.CurrentRoom.RoomMap.GetRoomTile(_item.Position.X, _item.Position.Y).WiredEffects)
            {
                foreach (BaseEntity entity in roomTile.Entities)
                {
                    effect.WiredInteraction.OnTrigger(entity);
                }
            }
        }

        public void OnCycle()
        {

        }
    }
}
