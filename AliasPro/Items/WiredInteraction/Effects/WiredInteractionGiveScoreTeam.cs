using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Games.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionGiveScoreTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE_TEAM;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionGiveScoreTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
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
                    foreach (BaseGame game in _item.CurrentRoom.Game.Games)
                    {
                        if (game.State == GameState.RUNNING && game.TimesGivenScore < MaxPoints)
                        {
                            if (game.TryGetTeam(TeamType, out IGameTeam team))
                            {
                                game.TimesGivenScore++;
                                team.TeamPoints += Points;
                                _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, team.TotalPoints);
                            }
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

        private GameTeamType TeamType
        {
            get
            {
                int teamId =
                    (WiredData.Params.Count <= 2) ? 0 : WiredData.Params[2];

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
