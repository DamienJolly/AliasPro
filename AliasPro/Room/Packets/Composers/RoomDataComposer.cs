using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Composers
{
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
            message.WriteBoolean(false); //staff picked
            message.WriteBoolean(false); //public room
            message.WriteBoolean(false); //muted
            message.WriteInt(_room.RoomData.Settings.WhoMutes);
            message.WriteInt(_room.RoomData.Settings.WhoKicks);
            message.WriteInt(_room.RoomData.Settings.WhoBans);
            message.WriteBoolean(true); //owner check
            message.WriteInt(_room.RoomData.Settings.ChatMode);
            message.WriteInt(_room.RoomData.Settings.ChatSize);
            message.WriteInt(_room.RoomData.Settings.ChatSpeed);
            message.WriteInt(_room.RoomData.Settings.ChatDistance);
            message.WriteInt(_room.RoomData.Settings.ChatFlood);
            return message;
        }
    }
}
