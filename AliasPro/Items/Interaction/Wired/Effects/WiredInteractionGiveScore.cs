using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Games.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionGiveScore : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredInteractionGiveScore(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
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

        public override void OnCycle()
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
                            Room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, _target.GamePlayer.Team.TotalPoints);
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
