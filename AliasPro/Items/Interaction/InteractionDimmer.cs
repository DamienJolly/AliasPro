using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionDimmer : ItemInteraction
    {
        public InteractionDimmer(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

        public async override void OnPlaceItem()
		{
            if (Item.CurrentRoom.Moodlight != null)
            {
                Item.ExtraData = Item.CurrentRoom.Moodlight.GenerateExtraData;
                await Item.CurrentRoom.SendPacketAsync(new WallItemUpdateComposer(Item));
            }
        }

		public override void OnPickupItem()
		{
            Item.ExtraData = "";
        }
    }
}
