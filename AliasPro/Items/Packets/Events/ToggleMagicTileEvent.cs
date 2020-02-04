using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class SetStackToolHeightEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.SetStackToolHeightMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
				return;

            if (session.Entity == null) 
				return;

			if (!room.Rights.HasRights(session.Player.Id)) 
				return;

            uint itemId = (uint)clientPacket.ReadInt();
			if (!room.Items.TryGetItem(itemId, out IItem item)) 
				return;

			if (!item.CurrentRoom.RoomGrid.TryGetRoomTile(item.Position.X, item.Position.Y, out IRoomTile itemTile))
				return;

			double stackerHeight = clientPacket.ReadInt();

			ICollection<IRoomTile> tiles = room.RoomGrid.GetTilesFromItem(item.Position.X, item.Position.Y, item);
			if (stackerHeight == -100)
			{
				foreach (IRoomTile tile in tiles)
				{
					double stackheight = StackHeight(tile) * 100;
					if (stackheight > stackerHeight)
						stackerHeight = stackheight;
				}
			}
			else
			{
				stackerHeight = Math.Min(Math.Max(stackerHeight, itemTile.Position.Z * 100), 4000);
			}

			double height = 0;
			if (stackerHeight >= 0)
			{
				height = stackerHeight / 100.0D;
			}

			room.RoomGrid.RemoveItem(item);
			item.Position.Z = height;
			room.RoomGrid.AddItem(item);

			await room.SendPacketAsync(new FloorItemUpdateComposer(item));
			await room.SendPacketAsync(new UpdateStackToolHeightComposer(item));
		}


		//todo: Remove this?
		private double StackHeight(IRoomTile tile)
		{
			double height = tile.Position.Z;

			IItem topItem = null;
			foreach (IItem item in tile.Items)
			{
				if (item.ItemData.InteractionType == ItemInteractionType.STACK_TOOL)
					continue;

				if (topItem == null ||
					(item.Position.Z + item.ItemData.Height) >
					(topItem.Position.Z + topItem.ItemData.Height))
				{
					topItem = item;
				}
			}

			if (topItem != null)
			{
				if (topItem.ItemData.InteractionType == ItemInteractionType.MULTIHEIGHT)
				{
					IList<double> heights = new List<double>();
					foreach (string data in topItem.ItemData.ExtraData.Split(','))
					{
						if (double.TryParse(data, out double heightData))
							heights.Add(heightData);
					}

					if (topItem.Mode <= heights.Count)
						return heights[topItem.Mode] + topItem.Position.Z;
				}

				height += topItem.ItemData.Height + topItem.Position.Z;
			}

			return height;
		}
    }
}
