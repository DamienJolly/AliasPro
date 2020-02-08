using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserStopTypingEvent : IMessageEvent
    {
        public short Header => Incoming.UserStopTypingMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return;

            await room.SendPacketAsync(new UserTypingComposer(session.Entity.Id, false));
        }
    }
}
