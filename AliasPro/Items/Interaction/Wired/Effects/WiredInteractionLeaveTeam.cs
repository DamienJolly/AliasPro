using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionLeaveTeam : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.LEAVE_TEAM;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredInteractionLeaveTeam(IItem item)
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
                        _target is PlayerEntity playerEntity)
                    {
                        if (playerEntity.GamePlayer != null)
                            playerEntity.GamePlayer.Game.LeaveTeam(playerEntity);
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
