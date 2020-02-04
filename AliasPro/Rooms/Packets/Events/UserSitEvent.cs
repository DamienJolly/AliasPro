using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserSitEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UserSitMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return Task.CompletedTask;

            if (session.Entity.Actions.HasStatus("mv") ||
                session.Entity.Actions.HasStatus("lay") ||
                session.Entity.Actions.HasStatus("sit"))
                return Task.CompletedTask;

            if ((session.Entity.BodyRotation % 2) != 0)
                session.Entity.SetRotation(session.Entity.BodyRotation - 1);

            session.Entity.Actions.AddStatus("sit", 0.5 + "");
            session.Entity.IsSitting = true;
            session.Entity.NeedsUpdate = true;

            return Task.CompletedTask;
        }
    }
}
