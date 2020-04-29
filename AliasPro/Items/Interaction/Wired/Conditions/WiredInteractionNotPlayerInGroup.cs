using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotPlayerInGroup : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_ACTOR_IN_GROUP;

        public WiredInteractionNotPlayerInGroup(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (Room.Group == null)
                return false;

            int groupId = Room.Group.Id;

            if (args.Length == 0)
                return false;

            PlayerEntity target = (PlayerEntity)args[0];
            if (target == null) 
                return false;

            return !target.Player.HasGroup(groupId);
        }
    }
}
