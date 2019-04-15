using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserActionEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserActionMessageEvent;

        public async void HandleAsync(
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
                session.Entity.Unidle();
            }

            await room.SendAsync(new UserActionComposer(session.Entity, action));
        }
    }
}
