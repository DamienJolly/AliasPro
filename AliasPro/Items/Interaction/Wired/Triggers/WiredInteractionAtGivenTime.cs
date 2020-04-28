using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionAtGivenTime : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.AT_GIVEN_TIME;

        private int _tick = 0;

        public WiredInteractionAtGivenTime(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            //todo: check if this is right
            _tick++;
            if (_tick < Timer)
                return false;

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
                Room.Items.TriggerEffects(roomTile);

            _tick = 0;
            return true;
        }

        private int Timer =>
            (WiredData.Params.Count != 1) ? 10 : WiredData.Params[0];
    }
}
