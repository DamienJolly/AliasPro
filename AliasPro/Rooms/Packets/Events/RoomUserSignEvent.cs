using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserSignEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RoomUserSignMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return Task.CompletedTask;

            int signId = clientPacket.ReadInt();

            if (signId < 0 || signId > 17)
                return Task.CompletedTask;

            if (signId == 0)
            {
                session.Entity.Actions.RemoveStatus("sign");
            }
            else
            {
                session.Entity.Actions.AddStatus("sign", signId + "");
                session.Entity.SignTimer = 10;
            }

            session.Entity.Unidle();
            session.Entity.NeedsUpdate = true;

            return Task.CompletedTask;
        }
    }
}
