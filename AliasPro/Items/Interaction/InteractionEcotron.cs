using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionEcotron : ItemInteraction
	{
		public readonly int itemId = -1;
		public readonly string ExtraData = "1-1-1997";

		public InteractionEcotron(IItem item)
			: base(item)
		{
			if (!string.IsNullOrEmpty(Item.ExtraData))
			{
				string[] data = Item.ExtraData.Split("\t");

				itemId = int.Parse(data[0]);
				ExtraData = data[1];
			}
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(ExtraData);
		}
    }
}
