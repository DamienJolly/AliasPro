using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Outgoing;

    public class UserActionEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserActionMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int action = clientPacket.ReadInt();
            if (action < 0 || action > 5) return;

            if (session.Entity.DanceId > 0)
            {
                session.Entity.DanceId = 0;
                await room.SendAsync(new UserDanceComposer(session.Entity));
            }

            //todo: remove effect

            if (action == 5)
            {
                if (!session.Entity.IsIdle)
                {
                    session.Entity.IsIdle = true;
                    await room.SendAsync(new UserSleepComposer(session.Entity));
                }
            }
            else
            {
                room.EntityHandler.Unidle(session.Entity);
            }

            await room.SendAsync(new UserActionComposer(session.Entity, action));
        }
    }
}
