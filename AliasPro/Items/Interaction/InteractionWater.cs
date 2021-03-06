﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionWater : ItemInteraction
	{
        public InteractionWater(IItem item)
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
			HandleWater();
		}

		public override void OnPickupItem()
		{
			OnMoveItem();
		}

		public override void OnMoveItem()
		{
			HandleWater();
		}

		private async void HandleWater()
		{
			foreach (IItem waterItem in Item.CurrentRoom.Items.FloorItems)
			{
				if (waterItem?.CurrentRoom == null) continue;

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
				await waterItem.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(waterItem));
			}
		}
    }
}
