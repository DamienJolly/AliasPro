using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionPressureTile : ItemInteraction
	{
        public InteractionPressureTile(IItem item)
			: base(item)
		{

		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public override void OnPlaceItem()
		{
			Item.ExtraData = "0";
		}

		public override void OnMoveItem()
		{
			Item.ExtraData = "0";
		}

		public async override void OnUserWalkOn(BaseEntity entity)
        {
			Item.ExtraData = "1";
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}

        public async override void OnUserWalkOff(BaseEntity entity)
        {
			Item.ExtraData = "0";
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}
    }
}
