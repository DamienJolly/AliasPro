namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomEntryInfoComposer : ServerPacket
    {
        public RoomEntryInfoComposer(uint roomId, bool hasRights)
            : base(Outgoing.RoomEntryInfoMessageComposer)
        {
            WriteInt(roomId);
            WriteBoolean(hasRights); //todo: user rights
        }
    }
}
