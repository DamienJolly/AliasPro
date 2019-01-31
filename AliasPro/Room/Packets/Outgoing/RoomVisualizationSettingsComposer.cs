namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomVisualizationSettingsComposer : ServerPacket
    {
        public RoomVisualizationSettingsComposer(bool hideWalls, int wallThickness, int floorThicknes)
            : base(Outgoing.RoomVisualizationSettingsMessageComposer)
        {
            WriteBoolean(hideWalls);
            WriteInt(wallThickness);
            WriteInt(floorThicknes);
        }
    }
}
