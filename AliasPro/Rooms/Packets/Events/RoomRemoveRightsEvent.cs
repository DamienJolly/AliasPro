using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomRemoveRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomRemoveRightsMessageEvent;

        private readonly IRoomController _roomController;

        public RoomRemoveRightsEvent( 
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

            if (room.OwnerId == session.Player.Id || room.Group != null) return;

            if (room.Loaded)
            {
                room.Rights.RemoveRights(session.Player.Id);

                await room.Rights.ReloadRights(session);
                await room.SendAsync(new RoomRemoveRightsListComposer((int)room.Id, (int)session.Player.Id));
            }

            await _roomController.TakeRoomRights(room.Id, session.Player.Id);
        }
    }
}

