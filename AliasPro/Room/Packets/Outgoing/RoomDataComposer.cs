namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Room.Models;
    using Sessions;

    public class RoomDataComposer : IPacketComposer
    {
        private readonly IRoom _room;
        private readonly bool _loading;
        private readonly bool _entry;
        private readonly ISession _session;

        public RoomDataComposer(IRoom room, bool loading, bool entry, ISession session)
        {
            _room = room;
            _loading = loading;
            _entry = entry;
            _session = session;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomDataMessageComposer);
            message.WriteBoolean(_loading);
            _room.RoomData.Compose(message);
            message.WriteBoolean(_entry);
            message.WriteBoolean(false);
            message.WriteBoolean(false);
            message.WriteBoolean(false);
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteInt(0);
            message.WriteBoolean(_session.Player.Id == _room.RoomData.OwnerId);
            message.WriteInt(0);
            message.WriteInt(1);
            message.WriteInt(1);
            message.WriteInt(50);
            message.WriteInt(2);
            return message;
        }
    }
}
