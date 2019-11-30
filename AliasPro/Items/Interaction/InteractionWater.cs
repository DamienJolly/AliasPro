using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionWater : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionWater(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }

		public void OnPlaceItem()
		{
			HandleWater();
		}

		public void OnPickupItem()
		{
			OnMoveItem();
		}

		public void OnMoveItem()
		{
			HandleWater();
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

		private async void HandleWater()
		{
			foreach (IItem waterItem in _item.CurrentRoom.Items.FloorItems)
			{
				if (waterItem.ItemData.InteractionType != Types.ItemInteractionType.WATER) continue;

				int result = 0;
				int xCount = -1;
				int yCount = -1;

				for (int i = 0; i < 12; i++)
				{
					int value = 0;
					if (waterItem.CurrentRoom.RoomGrid.TryGetRoomTile(waterItem.Position.X + xCount, waterItem.Position.Y + yCount, out IRoomTile tile))
					{
						if (tile.TopItem != null && tile.TopItem.ItemData.InteractionType == Types.ItemInteractionType.WATER)
							value = 1;
					}
					else if (yCount != -1 && yCount != 2 || xCount != -1 && xCount != 2)
						value = 1;

					result |= value << (11 - i);

					xCount++;
					if (yCount == 0 || yCount == 1)
						xCount += 2;

					if (xCount >= 3)
					{
						yCount++;
						xCount = -1;
					}
				}

				waterItem.ExtraData = result.ToString();
				System.Console.WriteLine(waterItem.ExtraData);
				await waterItem.CurrentRoom.SendAsync(new FloorItemUpdateComposer(waterItem));
			}
		}
    }
}
