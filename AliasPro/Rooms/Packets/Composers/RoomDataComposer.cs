using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomDataComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomDataMessageComposer);
            message.WriteBoolean(_loading);
            _room.Compose(message);
			message.WriteBoolean(_entry);
            message.WriteBoolean(false); //staff picked
            message.WriteBoolean(false); //public room
            message.WriteBoolean(_room.Muted);
            message.WriteInt(_room.Settings.WhoMutes);
            message.WriteInt(_room.Settings.WhoKicks);
            message.WriteInt(_room.Settings.WhoBans);
            message.WriteBoolean(_session.Player.Id == _room.OwnerId);
            message.WriteInt(_room.Settings.ChatMode);
            message.WriteInt(_room.Settings.ChatSize);
            message.WriteInt(_room.Settings.ChatSpeed);
            message.WriteInt(_room.Settings.ChatDistance);
            message.WriteInt(_room.Settings.ChatFlood);
            return message;
        }
    }
}
