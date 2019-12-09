using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorPlanDoorSettingsComposer : IPacketComposer
    {
        private readonly IRoom _room;

        public FloorPlanDoorSettingsComposer(IRoom room)
        {
            _room = room;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FloorPlanDoorSettingsMessageComposer);
            message.WriteInt(_room.RoomModel.DoorX);
            message.WriteInt(_room.RoomModel.DoorY);
            message.WriteInt(_room.RoomModel.DoorDir);
            return message;
        }
    }
}
