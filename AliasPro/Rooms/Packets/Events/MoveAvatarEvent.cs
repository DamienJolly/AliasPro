using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class MoveAvatarEvent : IMessageEvent
    {
        public short Header => Incoming.MoveAvatarMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
				return Task.CompletedTask;

			if (session.Entity.IsKicked)
				return Task.CompletedTask;

			int x = message.ReadInt();
            int y = message.ReadInt();

			IRoomTile roomTile;
            if (!room.RoomGrid.TryGetRoomTile(x, y, out roomTile))
				return Task.CompletedTask;

			var topItem = roomTile.TopItem;

			if (topItem != null && topItem.ItemData.InteractionType == Items.Types.ItemInteractionType.BED)
			{
				if ((topItem.Rotation % 4) != 0)
				{
					if (x != topItem.Position.X)
						x = topItem.Position.X;
				}
				else
				{
					if (y != topItem.Position.Y)
						y = topItem.Position.Y;
				}

				if (!room.RoomGrid.TryGetRoomTile(x, y, out roomTile))
					return Task.CompletedTask;
			}

			if (!roomTile.IsValidTile(session.Entity, true))
				return Task.CompletedTask;

			session.Entity.GoalPosition = roomTile.Position;
			return Task.CompletedTask;
		}
    }
}
