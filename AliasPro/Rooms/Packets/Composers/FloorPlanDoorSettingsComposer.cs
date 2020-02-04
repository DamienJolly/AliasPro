using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorPlanDoorSettingsComposer : IMessageComposer
    {
        private readonly IRoom _room;

        public FloorPlanDoorSettingsComposer(IRoom room)
        {
            _room = room;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FloorPlanDoorSettingsMessageComposer);
            message.WriteInt(_room.RoomModel.DoorX);
            message.WriteInt(_room.RoomModel.DoorY);
            message.WriteInt(_room.RoomModel.DoorDir);
            return message;
        }
    }
}
