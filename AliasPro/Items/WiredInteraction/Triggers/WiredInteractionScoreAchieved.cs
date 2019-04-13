using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionScoreAchieved : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredTriggerType _type = WiredTriggerType.SCORE_ACHIEVED;
        
        public IWiredData WiredData { get; set; }

        public WiredInteractionScoreAchieved(IItem item)
        {
            _item = item;
            WiredData = 
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (args.Length <= 0) return false;

            int score = (int)args[0];
            if (score < ScoreToGet) return false;

            if (_item.CurrentRoom.Mapping.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile roomTile))
            {
                _item.CurrentRoom.Items.TriggerEffects(roomTile);
            }
            return true;
        }

        public void OnCycle()
        {

        }

        private int ScoreToGet =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];
    }
}
