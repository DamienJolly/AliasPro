using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionCollision : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.COLLISION;
        
        public WiredInteractionCollision(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            IRoomPosition position = (IRoomPosition)args[0];

            if (Room.RoomGrid.TryGetRoomTile(position.X, position.Y, out IRoomTile roomTile))
            {
                foreach (BaseEntity entity in roomTile.Entities)
                {
                    Room.Items.TriggerEffects(roomTile, entity);
                }
            }
            return true;
        }
    }
}
