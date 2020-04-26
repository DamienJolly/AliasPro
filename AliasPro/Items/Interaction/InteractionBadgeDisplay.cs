using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionBadgeDisplay : ItemInteraction
	{
		private readonly string _badgeCode = "";
		private readonly string _username = "Unknown Player";
		private readonly string _date = "Unknown Date";

		public InteractionBadgeDisplay(IItem item)
			: base(item)
		{
			if(!string.IsNullOrEmpty(Item.ExtraData))
			{
				string[] data = Item.ExtraData.Split(";");

				_badgeCode = data[0];
				_username = data[1];
				_date = data[2];
			}
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(2);
			message.WriteInt(4);
			message.WriteString("0");
			message.WriteString(_badgeCode);
			message.WriteString(_username);
			message.WriteString(_date);
		}
    }
}
