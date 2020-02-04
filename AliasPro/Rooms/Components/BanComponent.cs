using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Rooms.Components
{
    public class BanComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, IRoomBan> _bannedPlayers;

        public bool GameStarted = false;

        public BanComponent(
            IRoom room,
            IDictionary<int, IRoomBan> bannedPlayers)
        {
            _room = room;
            _bannedPlayers = bannedPlayers;
        }

        public bool TryBanPlayer(int playerId, IRoomBan roomBan) =>
            _bannedPlayers.TryAdd(playerId, roomBan);

        public async void UnbanPlayer(int playerId)
        {
            _bannedPlayers.Remove(playerId);
            await _room.SendPacketAsync(new RoomUserUnbannedComposer((int)_room.Id, playerId));
        }

        public bool PlayerBanned(int playerId)
        {
            bool isBanned = false;
            if (_bannedPlayers.TryGetValue(playerId, out IRoomBan roomBan))
            {
                isBanned = !roomBan.HasExpired;

                if (!isBanned)
                    UnbanPlayer(roomBan.PlayerId);
            }
            return isBanned;
        }

        public ICollection<IRoomBan> BannedPlayers
        {
            get
            {
                IList<IRoomBan> bannedPlayers = new List<IRoomBan>();
                foreach (IRoomBan roomBan in _bannedPlayers.Values)
                {
                    if (roomBan.HasExpired)
                    {
                        UnbanPlayer(roomBan.PlayerId);
                        continue;
                    }

                    bannedPlayers.Add(roomBan);
                }
                return bannedPlayers;
            }
        }
    }
}
