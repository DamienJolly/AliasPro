using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models
{
    using Network.Events;
    using Entities;
    using Gamemap;
    using Sessions;
    using AliasPro.Room.Models.Item;

    public interface IRoom
    {
        bool isLoaded { get; set; }
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        IDictionary<int, BaseEntity> Entities { get; }
        IDictionary<uint, IRoomItem> RoomItems { get; }
        void OnChat(string text, int colour, BaseEntity entity);
        void LeaveRoom(ISession session);
        void LoadRoomItems(IDictionary<uint, IRoomItem> items);
        void SetupRoomCycle();
        Task SendAsync(IPacketComposer serverPacket);
        Task AddEntity(BaseEntity entity);
    }
}
