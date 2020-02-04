using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomVisualizationSettingsComposer : IMessageComposer
    {
        private readonly IRoomSettings _settings;

        public RoomVisualizationSettingsComposer(IRoomSettings settings)
        {
            _settings = settings;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomVisualizationSettingsMessageComposer);
            message.WriteBoolean(_settings.HideWalls);
            message.WriteInt(_settings.WallThickness);
            message.WriteInt(_settings.FloorThickness);
            return message;
        }
    }
}
