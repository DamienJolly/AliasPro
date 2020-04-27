using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Games;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Games;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionJoinTeam : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.JOIN_TEAM;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredInteractionJoinTeam(IItem item)
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
                        if (!Room.Game.TryGetGame(GameType.WIRED, out BaseGame game))
                        {
                            game = new WiredGame(Room);
                            Room.Game.TryAddGame(game);
                        }

                        game.JoinTeam(playerEntity, TeamType);
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
