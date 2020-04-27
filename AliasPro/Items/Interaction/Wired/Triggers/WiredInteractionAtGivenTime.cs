using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionAtGivenTime : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.AT_GIVEN_TIME;

        private bool _active = false;
        private int _tick = 0;

        public WiredInteractionAtGivenTime(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = Timer;
            }
            return true;
        }

        public override void OnCycle()
        {
            if (_active)
            {
                _tick--;
                if (_tick <= 0)
                {
                    if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
                    {
                        Room.Items.TriggerEffects(roomTile);
                    }
                    _active = false;
                }
            }
        }

        private int Timer =>
            (WiredData.Params.Count != 1) ? 10 : WiredData.Params[0];
    }
}
