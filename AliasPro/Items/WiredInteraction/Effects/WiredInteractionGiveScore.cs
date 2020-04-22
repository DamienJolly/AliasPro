using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionGiveScore : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

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
                        _target.GamePlayer != null && 
                        _target.GamePlayer.Game.State == GameState.RUNNING)
                    {
                        if (_target.GamePlayer.Game.TimesGivenScore < MaxPoints)
                        {
                            _target.GamePlayer.Game.TimesGivenScore++;
                            _target.GamePlayer.Points += Points;
                            _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, _target.GamePlayer.Team.TotalPoints);
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private int Points =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];

        private int MaxPoints =>
            (WiredData.Params.Count <= 1) ? 1 : WiredData.Params[1];
    }
}
