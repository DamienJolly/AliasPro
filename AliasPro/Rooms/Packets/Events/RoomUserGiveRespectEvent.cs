using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserGiveRespectEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserGiveRespectMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            int playerId = message.ReadInt();

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity targetEntity))
                return;

            if (session.Player.Respects <= 0)
                return;

            targetEntity.Player.RespectsRecieved++;
            session.Player.Respects--;
            session.Player.RespectsGiven++;

            await room.SendPacketAsync(new RoomUserRespectComposer(targetEntity.Player));
            await room.SendPacketAsync(new UserActionComposer(session.Entity, 7));

            // todo: some achievements

            // todo: unidle
        }
    }
}
