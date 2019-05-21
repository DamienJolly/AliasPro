using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRoomAlertEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRoomAlertMessageEvent;
        
        private readonly IRoomController _roomController;

        public ModerationRoomAlertEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;

            if (session.CurrentRoom == null)
                return;

            clientPacket.ReadInt();
            string message = clientPacket.ReadString();

            await session.CurrentRoom.SendAsync(new ModerationIssueHandledComposer(message));
        }
    }
}