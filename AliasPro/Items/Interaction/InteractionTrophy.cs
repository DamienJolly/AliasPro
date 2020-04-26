using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionTrophy : ItemInteraction
	{
        public InteractionTrophy(IItem item)
			: base(item)
        {

        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);

			if (!string.IsNullOrEmpty(Item.ExtraData))
			{
				string[] data = Item.ExtraData.Split(";");

				message.WriteString(data[0] + (char)9 + data[1] + (char)9 + data[2]);
			}
			else
			{
				message.WriteString("Unknown" + (char)9 + "1-1-1970" + (char)9 + string.Empty);
			}
		}
    }
}
