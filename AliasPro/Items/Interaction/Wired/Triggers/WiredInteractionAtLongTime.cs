using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionAtLongTime : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.AT_GIVEN_TIME;

        public WiredInteractionAtLongTime(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
                Room.Items.TriggerEffects(roomTile);
            return true;
        }

        public override void ResetTimers()
        {
            Room.Items.TriggerWired(WiredInteractionType.AT_LONG_TIME);
        }

        public override int RequiredCooldown =>
            ((WiredData.Params.Count != 1) ? 1 : WiredData.Params[0]) * 500;
    }
}
