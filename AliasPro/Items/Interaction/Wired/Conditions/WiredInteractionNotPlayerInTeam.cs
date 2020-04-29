using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotPlayerInTeam : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_ACTOR_IN_TEAM;

        public WiredInteractionNotPlayerInTeam(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0)
                return false;

            PlayerEntity target = (PlayerEntity)args[0];
            if (target == null) 
                return false;

            if (target.GamePlayer == null)
                return true;

            return target.GamePlayer.Team.Type != TeamType;
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
