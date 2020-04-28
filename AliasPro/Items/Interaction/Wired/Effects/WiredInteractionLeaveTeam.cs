using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionLeaveTeam : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.LEAVE_TEAM;

        public WiredInteractionLeaveTeam(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length == 0) 
                return false;

            BaseEntity target = (BaseEntity)args[0];
            if (target is PlayerEntity playerEntity)
            {
                if (playerEntity.GamePlayer != null)
                    playerEntity.GamePlayer.Game.LeaveTeam(playerEntity);
            }
            return true;
        }
    }
}
