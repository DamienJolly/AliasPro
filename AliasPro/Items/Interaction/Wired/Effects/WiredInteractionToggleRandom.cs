using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionToggleRandom : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.TOGGLE_RANDOM;

        public WiredInteractionToggleRandom(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                //todo: maybe use interactor
                item.ExtraData = Randomness.RandomNumber(0, item.ItemData.Modes) + "";
                Room.SendPacketAsync(new FloorItemUpdateComposer(Item));
            }
            return true;
        }
    }
}
