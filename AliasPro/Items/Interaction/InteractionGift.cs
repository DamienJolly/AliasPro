using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionGift : IItemInteractor
    {
        private readonly IItem _item;

		private readonly IList<int> _itemIds = new List<int>();
		private readonly int _colorId = 0;
		private readonly int _ribbonId = 0;
		private readonly bool _showSender = false;
		private readonly string _message = "Please delete this present. Thanks!";
		private readonly string _sender = "Uknown";
		private readonly string _figure = "";

		public InteractionGift(IItem item)
        {
            _item = item;

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split("\t");

				int count = int.Parse(data[0]);
				for (int i = 0; i < count; i++)
				{
					_itemIds.Add(int.Parse(data[i + 1]));
				}

				_colorId = int.Parse(data[count + 1]);
				_ribbonId = int.Parse(data[count + 2]);
				_showSender = data[count + 3] == "1";
				_message = data[count + 4];
				_sender = data[count + 5];
				_figure = data[count + 6];
			}

		}

		public void Compose(ServerPacket message)
        {
			message.WriteInt(_colorId * 1000 + _ribbonId);
			message.WriteInt(1);
			message.WriteInt(6);
			message.WriteString("EXTRA_PARAM");
			message.WriteString("");
			message.WriteString("MESSAGE");
			message.WriteString(_message);
			message.WriteString("PURCHASER_NAME");
			message.WriteString(_showSender ? _sender : "");
			message.WriteString("PURCHASER_FIGURE");
			message.WriteString(_showSender ? _figure : "");
			message.WriteString("PRODUCT_CODE");
			message.WriteString("");
			message.WriteString("state");
			message.WriteString("0");

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
