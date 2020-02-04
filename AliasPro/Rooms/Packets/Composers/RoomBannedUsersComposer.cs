using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomBannedUsersComposer : IMessageComposer
    {
        private readonly int _roomId;
        private readonly ICollection<IRoomBan> _bannedPlayers;

        public RoomBannedUsersComposer(int roomId, ICollection<IRoomBan> bannedPlayers)
        {
            _roomId = roomId;
            _bannedPlayers = bannedPlayers;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomBannedUsersMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_bannedPlayers.Count);
            foreach (IRoomBan roomBan in _bannedPlayers)
            {
                message.WriteInt(roomBan.PlayerId);
                message.WriteString(roomBan.PlayerName);
            }
            return message;
        }
    }
}
