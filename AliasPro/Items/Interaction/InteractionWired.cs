using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionWired : ItemInteraction
    {
        public InteractionWired(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString("");
        }

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity is PlayerEntity playerEntity)
			{
				if (!Item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;

                if (Item.ItemData.InteractionType == ItemInteractionType.WIRED_TRIGGER)
                {
                    await playerEntity.Session.SendPacketAsync(new WiredTriggerDataComposer(Item));
                }
                else if (Item.ItemData.InteractionType == ItemInteractionType.WIRED_EFFECT)
                {
                    await playerEntity.Session.SendPacketAsync(new WiredEffectDataComposer(Item));
                }
                else if (Item.ItemData.InteractionType == ItemInteractionType.WIRED_CONDITION)
                {
                    await playerEntity.Session.SendPacketAsync(new WiredConditionDataComposer(Item));
                }
            }
        }

        public override void OnCycle()
        {
            if (Item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER ||
                Item.ItemData.WiredInteractionType == WiredInteractionType.REPEATER_LONG)
            {
                Item.WiredInteraction.OnTrigger();
            }

            Item.WiredInteraction.OnCycle();
        }
    }
}
