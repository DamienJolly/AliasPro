﻿using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Rooms
{
    public interface IRoomController
    {
        ICollection<IRoom> Rooms { get; }
        Task<ICollection<IRoomData>> GetPlayersRoomsAsync(uint playerId);
        Task<IRoomData> GetRoomDataAsync(uint roomId);
        bool TryAddRoom(IRoom room);
        void RemoveRoom(IRoom room);
        bool TryGetRoom(uint roomId, out IRoom room);

        bool TryGetRoomModel(string modelName, out IRoomModel model);

        Task<IRoomSettings> GetRoomSettingsAsync(uint roomId);
        Task CreateRoomSettingsAsync(uint roomId);

        Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId);
        Task GiveRoomRights(uint roomId, uint playerId);
        Task TakeRoomRights(uint roomId, uint playerId);
    }
}