using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestBannedUsersEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestBannedUsersMessageEvent;

        private readonly IRoomController _roomController;

        public RequestBannedUsersEvent(
			IRoomController roomController)
        {
            _roomController = roomController;
        }

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
            int roomId = clientPacket.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                return;

            await session.SendPacketAsync(new RoomBannedUsersComposer((int)room.Id, room.Bans.BannedPlayers));
        }
    }
}
