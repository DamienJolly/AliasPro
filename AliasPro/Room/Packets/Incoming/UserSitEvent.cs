using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Room.Gamemap;
    using Item.Models;
    using Outgoing;

    public class UserSitEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserSitMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            if (session.Entity.ActiveStatuses.ContainsKey("mv") ||
                session.Entity.ActiveStatuses.ContainsKey("lay")) return;

            if (!room.RoomMap.TryGetRoomTile(session.Entity.Position.X, session.Entity.Position.Y, out RoomTile roomTile)) return;

            IItem topItem = roomTile.TopItem;
            if (topItem != null && topItem.ItemData.CanSit) return;

            if (!session.Entity.ActiveStatuses.ContainsKey("sit"))
            {
                if ((session.Entity.BodyRotation % 2) != 0)
                {
                    session.Entity.BodyRotation--;
                    session.Entity.HeadRotation = 
                        session.Entity.BodyRotation;
                }

                session.Entity.ActiveStatuses.Add("sit", 0.5 + "");
                session.Entity.IsSitting = true;
            }

            await room.SendAsync(new EntityUpdateComposer(session.Entity));
        }
    }
}
