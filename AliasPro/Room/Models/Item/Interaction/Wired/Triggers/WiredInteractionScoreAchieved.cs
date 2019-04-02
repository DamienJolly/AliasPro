using AliasPro.Item.Models;
using AliasPro.Room.Gamemap;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionScoreAchieved : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SCORE_ACHIEVED;
        
        public WiredData WiredData { get; set; }

        public WiredInteractionScoreAchieved(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            if (args.Length <= 0) return;

            int score = (int)args[0];
            if (score < ScoreToGet) return;

            if (_item.CurrentRoom.RoomMap.TryGetRoomTile(_item.Position.X, _item.Position.Y, out RoomTile roomTile))
            {
                _item.CurrentRoom.ItemHandler.TriggerEffects(roomTile);
            }
        }

        public void OnCycle()
        {

        }

        private int ScoreToGet =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
