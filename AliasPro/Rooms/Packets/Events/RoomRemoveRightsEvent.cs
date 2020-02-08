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
    public class RoomRemoveRightsEvent : IMessageEvent
    {
        public short Header => Incoming.RoomRemoveRightsMessageEvent;

        private readonly IRoomController _roomController;

        public RoomRemoveRightsEvent( 
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

            if (room.OwnerId == session.Player.Id || room.Group != null) return;

            if (room.Loaded)
            {
                room.Rights.RemoveRights(session.Player.Id);

                await room.Rights.ReloadRights(session);
                await room.SendPacketAsync(new RoomRemoveRightsListComposer((int)room.Id, (int)session.Player.Id));
            }

            await _roomController.TakeRoomRights(room.Id, session.Player.Id);
        }
    }
}

