using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserActionEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UserActionMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int action = clientPacket.ReadInt();
            if (action < 0 || action > 5) return;

            if (session.Entity.DanceId > 0)
            {
                session.Entity.DanceId = 0;
                await room.SendPacketAsync(new UserDanceComposer(session.Entity));
            }

            //todo: remove effect

            if (action == 5)
            {
                if (!session.Entity.IsIdle)
                {
                    session.Entity.IsIdle = true;
                    await room.SendPacketAsync(new UserSleepComposer(session.Entity));
                }
            }
            else
            {
                session.Entity.Unidle();
            }

            await room.SendPacketAsync(new UserActionComposer(session.Entity, action));
        }
    }
}
