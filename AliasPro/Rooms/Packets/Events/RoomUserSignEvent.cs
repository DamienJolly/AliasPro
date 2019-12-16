using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserSignEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserSignMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            int signId = clientPacket.ReadInt();

            if (signId < 0 || signId > 17) return;

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
        }
    }
}
