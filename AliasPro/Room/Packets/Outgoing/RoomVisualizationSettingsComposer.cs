namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomVisualizationSettingsComposer : IPacketComposer
    {
        private readonly bool _hideWalls;
        private readonly int _wallThickness;
        private readonly int _floorThicknes;

        public RoomVisualizationSettingsComposer(bool hideWalls, int wallThickness, int floorThicknes)
        {
            _hideWalls = hideWalls;
            _wallThickness = wallThickness;
            _floorThicknes = floorThicknes;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomVisualizationSettingsMessageComposer);
            message.WriteBoolean(_hideWalls);
            message.WriteInt(_wallThickness);
            message.WriteInt(_floorThicknes);
            return message;
        }
    }
}
