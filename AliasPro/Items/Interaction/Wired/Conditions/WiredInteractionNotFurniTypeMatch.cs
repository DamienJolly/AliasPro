using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionNotFurniTypeMatch : WiredInteraction
    {
        private static readonly WiredConditionType _type = WiredConditionType.NOT_STUFF_IS;

        public WiredInteractionNotFurniTypeMatch(IItem item)
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

                if (targetItem.ItemData.Id != item.ItemData.Id)
                    return true;
            }

            return false;
        }
    }
}
