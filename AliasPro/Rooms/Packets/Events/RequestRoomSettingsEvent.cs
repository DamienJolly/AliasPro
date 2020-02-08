using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestRoomSettingsMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomSettingsEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int roomId = message.ReadInt();
            IRoom room = await _roomController.LoadRoom((uint)roomId);

            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id) return;

            await session.SendPacketAsync(new RoomSettingsComposer(room));
        }
    }
}
