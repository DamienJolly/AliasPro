using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Models;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomVisualizationSettingsComposer : IPacketComposer
    {
        private readonly IRoomSettings _settings;

        public RoomVisualizationSettingsComposer(IRoomSettings settings)
        {
            _settings = settings;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomVisualizationSettingsMessageComposer);
            message.WriteBoolean(_settings.HideWalls);
            message.WriteInt(_settings.WallThickness);
            message.WriteInt(_settings.FloorThickness);
            return message;
        }
    }
}
