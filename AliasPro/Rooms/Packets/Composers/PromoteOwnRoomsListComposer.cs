using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class PromoteOwnRoomsListComposer : IMessageComposer
    {
        private readonly ICollection<IRoomData> _rooms;

        public PromoteOwnRoomsListComposer(ICollection<IRoomData> rooms)
        {
            _rooms = rooms;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PromoteOwnRoomsListMessageComposer);
            message.WriteBoolean(false); //??
            message.WriteInt(_rooms.Count);
            foreach (IRoomData room in _rooms)
            {
                message.WriteInt((int)room.Id);
                message.WriteString(room.Name);
                message.WriteBoolean(room.IsPromoted);
            }
            return message;
        }
    }
}
