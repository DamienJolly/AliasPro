using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomBannedUsersComposer : IPacketComposer
    {
        private readonly int _roomId;
        private readonly ICollection<IRoomBan> _bannedPlayers;

        public RoomBannedUsersComposer(int roomId, ICollection<IRoomBan> bannedPlayers)
        {
            _roomId = roomId;
            _bannedPlayers = bannedPlayers;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomBannedUsersMessageComposer);
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
