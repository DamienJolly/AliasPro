using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;

namespace AliasPro.Room.Packets.Events
{
    public class UserSitEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserSitMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            if (session.Entity.Actions.HasStatus("mv") ||
                session.Entity.Actions.HasStatus("lay") ||
                session.Entity.Actions.HasStatus("sit")) return;
            
            if ((session.Entity.BodyRotation % 2) != 0)
            {
                session.Entity.BodyRotation--;
                session.Entity.HeadRotation =
                    session.Entity.BodyRotation;
            }

            session.Entity.Actions.AddStatus("sit", 0.5 + "");
            session.Entity.IsSitting = true;

            await room.SendAsync(new EntityUpdateComposer(session.Entity));
        }
    }
}
