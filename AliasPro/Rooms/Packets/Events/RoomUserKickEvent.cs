using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserKickEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserKickMessageEvent;

        private readonly IMessengerController _messengerController;

        public RoomUserKickEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            int playerId = clientPacket.ReadInt();

            if (!room.Rights.HasRights(session.Player.Id)) return;

            if (room.Rights.HasRights((uint)playerId)) return;

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
                return;

            if (room.RoomModel.IsCustom /*|| room.IsPublic  */)
            {
                await room.RemoveEntity(entity);
            }
            else
            {
                entity.Session.CurrentRoom = null;
                entity.IsKicked = true;

                entity.GoalPosition = new RoomPosition(
                    room.RoomModel.DoorX, 
                    room.RoomModel.DoorY, 
                    room.RoomModel.DoorZ
                );
            }

            //habbo.getClient().sendResponse(new GenericErrorMessagesComposer(GenericErrorMessagesComposer.KICKED_OUT_OF_THE_ROOM));

            if (entity.Session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(entity.Session.Player, entity.Session.Player.Messenger.Friends);
        }
    }
}
