using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionFurniTypeMatch : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.STUFF_IS;

        public WiredInteractionFurniTypeMatch(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            if (args.Length <= 1)
                return false;

            IItem targetItem = (IItem)args[1];
            if (targetItem == null) 
                return false;

            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item))
                    continue;

                if (targetItem.ItemData.Id == item.ItemData.Id)
                    return true;
            }

            return false;
        }
    }
}
