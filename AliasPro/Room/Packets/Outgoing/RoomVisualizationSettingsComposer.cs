namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

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
