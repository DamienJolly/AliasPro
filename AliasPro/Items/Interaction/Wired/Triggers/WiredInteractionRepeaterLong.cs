using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionRepeaterLong : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.PERIODICALLY_LONG;

        private int _tick = 0;
        
        public WiredInteractionRepeaterLong(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            _tick++;
            if (_tick < Timer)
                return false;

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
                Room.Items.TriggerEffects(roomTile);

            _tick = 0;
            return true;
        }

        private int Timer =>
            ((WiredData.Params.Count != 1) ? 1 : WiredData.Params[0]) * 10;
    }
}
