using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionActorInTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredConditionType _type = WiredConditionType.ACTOR_IN_TEAM;

        public IWiredData WiredData { get; set; }

        public WiredInteractionActorInTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public bool OnTrigger(params object[] args)
        {
            if (args.Length == 0) return false;

            BaseEntity entity = (BaseEntity)args[0];
            if (entity == null) return false;

            if (entity.Team == GameTeamType.NONE) return false;

            if (entity.Team != TeamType) return false;

            return true;
        }

        public void OnCycle()
        {

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
