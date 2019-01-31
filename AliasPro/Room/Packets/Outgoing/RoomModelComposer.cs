namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomModelComposer : ServerPacket
    {
        public RoomModelComposer(string modelName, uint roomId)
            : base(Outgoing.RoomModelMessageComposer)
        {
            WriteString(modelName);
            WriteInt(roomId);
        }
    }
}
