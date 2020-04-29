using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Utilities;
using System;

namespace AliasPro.Items.Interaction
{
	public class InteractionWiredPyramid : ItemInteraction
	{
		public int NextChange = 0;

		public InteractionWiredPyramid(IItem item)
			: base(item)
		{

		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public async void ChangeState()
		{
			if (!(Item.ExtraData == "0" || Item.ExtraData == "1"))
				Item.ExtraData = "0";

			if (Room.RoomGrid.HasEntities(Item.Position.X, Item.Position.Y))
				return;

			int.TryParse(Item.ExtraData, out int state);
			state = Math.Abs(state - 1);

			Item.ExtraData = $"{state}";
			await Room.SendPacketAsync(new FloorItemUpdateComposer(Item));
			NextChange = (int)UnixTimestamp.Now + 1 + Randomness.RandomNumber(18);
		}
	}
}
