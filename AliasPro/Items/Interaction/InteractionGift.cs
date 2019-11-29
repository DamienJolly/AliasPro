﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionGift : IItemInteractor
    {
        private readonly IItem _item;

		public readonly IList<int> ItemIds = new List<int>();
		public readonly int ColorId = 0;
		public readonly int RibbonId = 0;
		public readonly bool ShowSender = false;
		public readonly string Message = "Please delete this present. Thanks!";
		public readonly string Sender = "Uknown";
		public readonly string Figure = "";
		public bool Exploaded = false;

		public InteractionGift(IItem item)
        {
            _item = item;

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split("\t");

				int count = int.Parse(data[0]);
				for (int i = 0; i < count; i++)
				{
					ItemIds.Add(int.Parse(data[i + 1]));
				}

				ColorId = int.Parse(data[count + 1]);
				RibbonId = int.Parse(data[count + 2]);
				ShowSender = data[count + 3] == "1";
				Message = data[count + 4];
				Sender = data[count + 5];
				Figure = data[count + 6];
			}

		}

		public void Compose(ServerPacket message, bool tradeItem)
		{
			System.Console.WriteLine(_item.ItemData.Name);
			if (!tradeItem)
				message.WriteInt(ColorId * 1000 + RibbonId);
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

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
