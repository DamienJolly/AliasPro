using AliasPro.API.Messenger;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserKickEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserKickMessageEvent;

        private readonly IMessengerController _messengerController;

        public RoomUserKickEvent(
            IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            int playerId = clientPacket.ReadInt();

            switch (room.Settings.WhoKicks)
            {
                case 0: default: if (!room.Rights.IsOwner(session.Player.Id)) return; break;
                case 1: if (!room.Rights.HasRights(session.Player.Id)) return; break;
                case 2: break;
            }

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

            await entity.Session.SendPacketAsync(new GenericErrorComposer(GenericErrorComposer.KICKED_OUT_OF_THE_ROOM));

            if (entity.Session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(entity.Session.Player, entity.Session.Player.Messenger.Friends);
        }
    }
}
