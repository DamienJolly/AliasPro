using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationUserRoomVisitsComposer : IPacketComposer
    {
        private readonly IPlayerData _playerData;
        private readonly ICollection<IPlayerRoomVisited> _roomVisits;

        public ModerationUserRoomVisitsComposer(IPlayerData playerData, ICollection<IPlayerRoomVisited> roomVisits)
        {
            _playerData = playerData;
            _roomVisits = roomVisits;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationUserRoomVisitsMessageComposer);
            message.WriteInt(_playerData.Id);
            message.WriteString(_playerData.Username);
            message.WriteInt(_roomVisits.Count);
            foreach (IPlayerRoomVisited visit in _roomVisits)
                visit.Compose(message);
            return message;
        }
    }
}
