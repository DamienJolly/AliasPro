using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Models.Game;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionJoinTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.JOIN_TEAM;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionJoinTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            if (!_active)
            {
                if (args.Length == 0) return;

                _active = true;
                _target = (BaseEntity)args[0];
                _tick = WiredData.Delay;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null &&
                        _target is UserEntity)
                    {
                        if (_target.Team != GameTeamType.NONE)
                            _item.CurrentRoom.GameHandler.LeaveTeam(_target);

                        _item.CurrentRoom.GameHandler.JoinTeam(_target, TeamType);
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private GameTeamType TeamType
        {
            get
            {
                int teamId =
                    (WiredData.Params.Count <= 0) ? 0 : WiredData.Params[0];

                switch (teamId)
                {
                    case 1: return GameTeamType.RED;
                    case 2: return GameTeamType.GREEN;
                    case 3: return GameTeamType.BLUE;
                    case 4: return GameTeamType.YELLOW;
                    default: return GameTeamType.NONE;
                }
            }
        }
    }
}
