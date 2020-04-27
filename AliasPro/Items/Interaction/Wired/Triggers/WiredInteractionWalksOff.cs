using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionWalksOff : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.WALKS_OFF_FURNI;

        public WiredInteractionWalksOff(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            IItem item = (IItem)args[1];
            if (item == null) return false;

            if (!WiredData.Items.ContainsKey(item.Id)) return false;

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
            {
                Room.Items.TriggerEffects(roomTile, entity);
            }
            return true;
        }
    }
}
