using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionScoreAchieved : WiredInteraction
    {
        private static readonly WiredTriggerType _type = WiredTriggerType.SCORE_ACHIEVED;
        
        public WiredInteractionScoreAchieved(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length <= 0) return false;

            int score = (int)args[0];
            if (score < ScoreToGet) return false;

            if (Room.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile roomTile))
            {
                Room.Items.TriggerEffects(roomTile);
            }
            return true;
        }

        private int ScoreToGet =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
