using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Types;

namespace AliasPro.Items.Interaction
{
    public abstract class InteractionWired : ItemInteraction
    {
        public InteractionWired(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

        public override void OnPickupItem()
        {
            Item.ExtraData = "0";
        }

        public override void OnCycle()
        {
            if (Item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER ||
                Item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER_LONG)
            {
                Item.WiredInteraction.Execute();
            }
        }
    }
}
