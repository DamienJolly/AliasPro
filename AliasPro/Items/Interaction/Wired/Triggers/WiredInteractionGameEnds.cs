using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionGameEnds : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.GAME_ENDS;

        public WiredInteractionGameEnds(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
            {
                Room.Items.TriggerEffects(roomTile, args);
            }
            return true;
        }
    }
}
