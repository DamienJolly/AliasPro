﻿using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionGift : ItemInteraction
	{
		public readonly int itemId = -1;
		public readonly string ExtraData = "";
		public readonly int ColorId = 0;
		public readonly int RibbonId = 0;
		public readonly bool ShowSender = false;
		public readonly string Message = "Please delete this present. Thanks!";
		public readonly string Sender = "Uknown";
		public readonly string Figure = "";
		public bool Exploaded = false;

		public InteractionGift(IItem item)
			: base(item)
        {
			if (!string.IsNullOrEmpty(Item.ExtraData))
			{
				string[] data = Item.ExtraData.Split("\t");

				itemId = int.Parse(data[0]);
				ExtraData = data[1];
				ColorId = int.Parse(data[2]);
				RibbonId = int.Parse(data[3]);
				ShowSender = data[4] == "1";
				Message = data[5];
				Sender = data[6];
				Figure = data[7];
			}
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(1);
			message.WriteInt(6);
			message.WriteString("EXTRA_PARAM");
			message.WriteString("");
			message.WriteString("MESSAGE");
			message.WriteString(Message);
			message.WriteString("PURCHASER_NAME");
			message.WriteString(ShowSender ? Sender : "");
			message.WriteString("PURCHASER_FIGURE");
			message.WriteString(ShowSender ? Figure : "");
			message.WriteString("PRODUCT_CODE");
			message.WriteString("");
			message.WriteString("state");
			message.WriteString(Exploaded ? "1" : "0");
		}
    }
}
