using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
{
    public class MoveAvatarEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.MoveAvatarMessageEvent;
        
        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int x = clientPacket.ReadInt();
            int y = clientPacket.ReadInt();

			IRoomTile roomTile;
            if (!room.RoomGrid.TryGetRoomTile(x, y, out roomTile))
                return;

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
					return;
			}

			if (!roomTile.IsValidTile(session.Entity, true))
                return;

			session.Entity.GoalPosition = roomTile.Position;
        }
    }
}
