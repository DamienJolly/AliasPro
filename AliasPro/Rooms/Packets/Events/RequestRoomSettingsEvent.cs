using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomSettingsMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomSettingsEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();
            IRoom room = await _roomController.LoadRoom((uint)roomId);

            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id) return;

            await session.SendPacketAsync(new RoomSettingsComposer(room));
        }
    }
}
