namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Room.Models;
    using Sessions;

    public class RoomDataComposer : ServerPacket
    {
        public RoomDataComposer(IRoom room, bool loading, bool entry, ISession session)
            : base(Outgoing.RoomDataMessageComposer)
        {
            WriteBoolean(loading);
            room.RoomData.Compose(this);
            WriteBoolean(entry);
            WriteBoolean(false);
            WriteBoolean(false);
            WriteBoolean(false);
            WriteInt(0);
            WriteInt(0);
            WriteInt(0);
            WriteBoolean(session.Player.Id == room.RoomData.OwnerId);
            WriteInt(0);
            WriteInt(1);
            WriteInt(1);
            WriteInt(50);
            WriteInt(2);
        }
    }
}
