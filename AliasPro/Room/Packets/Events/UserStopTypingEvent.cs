using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;

namespace AliasPro.Room.Packets.Events
{
    public class UserStopTypingEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserStopTypingMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            await room.SendAsync(new UserTypingComposer(session.Entity.Id, false));
        }
    }
}
