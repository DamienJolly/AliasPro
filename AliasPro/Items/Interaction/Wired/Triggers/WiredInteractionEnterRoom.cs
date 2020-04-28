using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionEnterRoom : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.ENTER_ROOM;
        
        public WiredInteractionEnterRoom(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length != 1) return false;

            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
            {
                Room.Items.TriggerEffects(roomTile, entity);
            }
            return true;
        }
    }
}
