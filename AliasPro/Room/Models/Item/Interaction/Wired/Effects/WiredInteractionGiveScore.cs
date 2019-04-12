using AliasPro.Items.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Models.Game;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionGiveScore : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionGiveScore(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                if (args.Length == 0) return false;
                
                _active = true;
                _target = (BaseEntity)args[0];
                _tick = WiredData.Delay;
            }
            return true;
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null && 
                        _target.Team != GameTeamType.NONE)
                    {
                        _item.CurrentRoom.GameHandler.GiveTeamPoints(_target.Team, TeamPoints, MaxPoints);
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private int TeamPoints =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];

        private int MaxPoints =>
            (WiredData.Params.Count <= 1) ? 1 : WiredData.Params[1];
    }
}
